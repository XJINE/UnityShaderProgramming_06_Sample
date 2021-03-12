using UnityEngine;
using System.Runtime.InteropServices;

public class Position : MonoBehaviour
{
    public  Material material;
    public  int      count = 30;

    ComputeBuffer buffer;

    void Start()
    {
        buffer = new ComputeBuffer(count, Marshal.SizeOf(typeof(Vector2)));

        Vector2[] positions = new Vector2[count];

        for (int i = 0; i < count; i++)
        {
            positions[i] = new Vector2(Random.value, Random.value);
        }

        buffer.SetData(positions);

        material.SetBuffer("_Buffer", buffer);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, material);
    }

    private void OnDestroy()
    {
        buffer.Dispose();
    }
}