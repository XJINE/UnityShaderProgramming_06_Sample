using System.Runtime.InteropServices;
using UnityEngine;

public class GraphicsDrawMeshInstancedIndirect : MonoBehaviour
{
    public Mesh     mesh;
    public Material material;
    public int      count = 10000;

    ComputeBuffer argsBuffer;
    ComputeBuffer matricesBuffer;

    uint[]      args;
    Matrix4x4[] matrices;

    void Start()
    {
        args    = new uint[5];
        args[0] = mesh.GetIndexCount(0);
        args[1] = (uint)count;
        args[2] = mesh.GetIndexStart(0);
        args[3] = mesh.GetBaseVertex(0);
        args[4] = 0;

        argsBuffer = new ComputeBuffer(args.Length, sizeof(uint), ComputeBufferType.IndirectArguments);
        argsBuffer.SetData(args);

        matrices = new Matrix4x4[count];

        for (int i = 0; i < count; i++)
        {
            matrices[i] = Matrix4x4.TRS(new Vector3(Random.Range(-3f, 3f),
                                                    Random.Range(-3f, 3f),
                                                    Random.Range(-3f, 3f)),
                       Quaternion.Euler(new Vector3(Random.Range(0, 360f),
                                                    Random.Range(0, 360f),
                                                    Random.Range(0, 360f))),
                                        new Vector3(0.1f, 0.1f, 0.1f));
        }

        matricesBuffer = new ComputeBuffer(count, Marshal.SizeOf(typeof(Matrix4x4)));
        matricesBuffer.SetData(matrices);

        material.SetBuffer("_MatricesBuffer", matricesBuffer);
    }

    void Update()
    {
        Graphics.DrawMeshInstancedIndirect
            (mesh, 0, material, new Bounds(Vector3.zero, Vector3.one * 100), argsBuffer);
    }

    void OnDestroy()
    {
        argsBuffer.Dispose();
        matricesBuffer.Dispose();
    }
}