using UnityEngine;

public class GraphicsDrawMesh : MonoBehaviour
{
    public Mesh     mesh;
    public Material material;
    public int      count = 10000;

    Matrix4x4[] matrices;

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
    }

    void Update()
    {
        for (int i = 0; i < count; i++)
        {
            Graphics.DrawMesh(mesh, matrices[i], material, 0);
        }
    }
}