using System.Runtime.InteropServices;
using UnityEngine;

public class GraphicsDrawMeshInstancedProcedural : MonoBehaviour
{
    public Mesh     mesh;
    public Material material;
    public int      count = 10000;

    ComputeBuffer matricesBuffer;
    Matrix4x4[]   matrices;

    void Start()
    {
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
        Graphics.DrawMeshInstancedProcedural
            (mesh, 0, material, new Bounds(Vector3.zero, Vector3.one * 100), count);
    }

    void OnDestroy()
    {
        matricesBuffer.Dispose();
    }
}