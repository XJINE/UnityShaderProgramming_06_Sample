using UnityEngine;

public class ComputeTexture : MonoBehaviour
{
    public ComputeShader computeShader;
           RenderTexture renderTexture;

    int        kernelIndex;
    Vector3Int kernelThreads;

    void Start()
    {
        renderTexture = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGB32);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        kernelIndex = computeShader.FindKernel("Kernel");

        computeShader.SetTexture(kernelIndex, "_Buffer", renderTexture);

        uint x, y, z;

        computeShader.GetKernelThreadGroupSizes(kernelIndex, out x, out y, out z);
        kernelThreads = new Vector3Int((int)x, (int)y, (int)z);
    }

    void Update()
    {
        computeShader.Dispatch(kernelIndex,
                               renderTexture.width  / kernelThreads.x, // - 20,
                               renderTexture.height / kernelThreads.y,
                               kernelThreads.z);
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(  0, 0, 512, 512), renderTexture);
    }

    void OnDestroy()
    {
        Destroy(renderTexture);
    }
}