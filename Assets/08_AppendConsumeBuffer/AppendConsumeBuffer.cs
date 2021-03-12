using System.Runtime.InteropServices;
using UnityEngine;

public class AppendConsumeBuffer : MonoBehaviour
{
    public struct Particle
    {
        public Vector3 position;
        public Vector3 velocity;
        public float   duration;
    }

    public ComputeShader computeShader;
    public Mesh          mesh;
    public Material      material;

    public int maxCount  = 100000;
    public int emitCount = 10;

    ComputeBuffer particleBuffer;
    ComputeBuffer pooledParticleBuffer;
    ComputeBuffer particleCountBuffer;

    Particle[] particles;
    uint[]     particleCount;

    int kernelIndexInitialize;
    int kernelIndexUpdate;
    int kernelIndexEmit;

    const int THREAD_NUM = 8;

    void Start()
    {
        maxCount  = (maxCount  / THREAD_NUM) * THREAD_NUM;
        emitCount = (emitCount / THREAD_NUM) * THREAD_NUM;

        kernelIndexInitialize = computeShader.FindKernel("Initialize");
        kernelIndexUpdate     = computeShader.FindKernel("Update");
        kernelIndexEmit       = computeShader.FindKernel("Emit");

        particleBuffer = new ComputeBuffer(maxCount, Marshal.SizeOf(typeof(Particle)));
        particles      = new Particle[maxCount];
        particleBuffer.SetData(particles);

        pooledParticleBuffer = new ComputeBuffer(maxCount, Marshal.SizeOf(typeof(uint)), ComputeBufferType.Append);
        pooledParticleBuffer.SetCounterValue(0);

        particleCountBuffer = new ComputeBuffer(1, Marshal.SizeOf(typeof(uint)), ComputeBufferType.IndirectArguments);
        particleCount       = new uint[] { 0 };
        particleCountBuffer.SetData(particleCount);

        computeShader.SetBuffer(kernelIndexUpdate,     "_ParticleBuffer",       particleBuffer);
        computeShader.SetBuffer(kernelIndexEmit,       "_ParticleBuffer",       particleBuffer);
        computeShader.SetBuffer(kernelIndexInitialize, "_DeadParticleBuffer",   pooledParticleBuffer);
        computeShader.SetBuffer(kernelIndexUpdate,     "_DeadParticleBuffer",   pooledParticleBuffer);
        computeShader.SetBuffer(kernelIndexEmit,       "_PooledParticleBuffer", pooledParticleBuffer);

        material.SetBuffer("_ParticleBuffer", particleBuffer);

        computeShader.Dispatch(kernelIndexInitialize, maxCount / THREAD_NUM, 1, 1);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Emit(Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10));
        }

        computeShader.SetFloat("_DeltaTime", Time.deltaTime);
        computeShader.Dispatch(kernelIndexUpdate, particleBuffer.count / THREAD_NUM, 1, 1);

        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, new Bounds(Vector3.zero, Vector3.one * 100), maxCount);
    }

    void Emit(Vector3 position)
    {
        ComputeBuffer.CopyCount(pooledParticleBuffer, particleCountBuffer, 0);

        particleCountBuffer.GetData(particleCount);

        if (particleCount[0] < emitCount)
        {
            return;
        }

        computeShader.SetVector("_ParticleEmitPosition", position);
        computeShader.SetFloat ("_ParticleEmitDuration", 3);

        computeShader.Dispatch(kernelIndexEmit, emitCount / THREAD_NUM, 1, 1);
    }

    void OnGUI()
    {
        ComputeBuffer.CopyCount(pooledParticleBuffer, particleCountBuffer, 0);
        particleCountBuffer.GetData(particleCount);
        GUILayout.Label("Pooled(Dead) Particles : " + particleCount[0]);
    }

    void OnDestroy()
    {
        particleBuffer      .Release();
        pooledParticleBuffer.Release();
        particleCountBuffer .Release();
    }
}