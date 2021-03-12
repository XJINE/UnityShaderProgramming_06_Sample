using UnityEngine;

public class ComputeArray : MonoBehaviour
{
    public ComputeShader computeShader;
           ComputeBuffer computeBuffer;

    void Start()
    {
        int indexA = computeShader.FindKernel("KernelA");
        int indexB = computeShader.FindKernel("KernelB");

        computeBuffer = new ComputeBuffer(4, sizeof(int));

        computeShader.SetBuffer(indexA, "_Buffer", computeBuffer);
        computeShader.SetInt("_Value", 1);
        computeShader.Dispatch(indexA, 1, 1, 1);

        int[] result = new int[4];

        computeBuffer.GetData(result);

        Debug.Log("RESULT : A");

        for (int i = 0; i < 4; i++)
        {
            Debug.Log(result[i]);
        }

        computeShader.SetBuffer(indexB, "_Buffer", computeBuffer);
        computeShader.Dispatch(indexB, 1, 1, 1);

        computeBuffer.GetData(result);

        Debug.Log("RESULT : B");

        for (int i = 0; i < 4; i++)
        {
            Debug.Log(result[i]);
        }

        computeBuffer.Release();
    }
}