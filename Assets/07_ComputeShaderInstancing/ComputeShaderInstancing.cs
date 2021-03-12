using System.Runtime.InteropServices;
using UnityEngine;

public class ComputeShaderInstancing : MonoBehaviour
{
    public struct Particle
    {
        public Vector3 position;
        public Vector3 velocity;
        public Color   color;
    }

    public ComputeShader computeShader;
    public Mesh          mesh;
    public Material      material;
    public int           count = 10000;

    ComputeBuffer particleBuffer;
    Particle[]    particles;

    int        kernelIndex;
    Vector3Int kernelThreads;

    void Start()
    {
        kernelIndex = computeShader.FindKernel("UpdateParticles");

        uint x, y, z;

        computeShader.GetKernelThreadGroupSizes(kernelIndex, out x, out y, out z);

        kernelThreads = new Vector3Int((int)x, (int)y, (int)z);

        particles = new Particle[count];

        for (int i = 0; i < count; i++)
        {
            particles[i] = new Particle()
            {
                position = new Vector3(Random.Range(-3f, 3f),
                                       Random.Range(-3f, 3f),
                                       Random.Range(-3f, 3f)) + transform.position,
                velocity = new Vector3(Random.Range(-3f, 3f),
                                       Random.Range(-3f, 3f),
                                       Random.Range(-3f, 3f)),
                color    = new Color  (Random.value, 
                                       Random.value, 
                                       Random.value)
            };
        }

        particleBuffer = new ComputeBuffer(count, Marshal.SizeOf(typeof(Particle)));
        particleBuffer.SetData(particles);

        computeShader.SetBuffer(kernelIndex, "_ParticleBuffer", particleBuffer);
        material.SetBuffer("_ParticleBuffer", particleBuffer);
    }

    void Update()
    {
        computeShader.SetFloat("_DeltaTime", Time.deltaTime);

        computeShader.Dispatch(kernelIndex, particleBuffer.count / kernelThreads.x, 1, 1);

        Graphics.DrawMeshInstancedProcedural
            (mesh, 0, material, new Bounds(Vector3.zero, Vector3.one * 100f), count);
    }

    void OnDestroy()
    {
        particleBuffer.Release();
    }
}