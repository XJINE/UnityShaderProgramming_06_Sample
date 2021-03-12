using UnityEngine;

public class VertexLimit : MonoBehaviour
{
    public Material material;
    public int      maxVerts  = 64008;
    const  int      cubeVerts = 24;

    [ContextMenu("Generate")]
    public void Generate()
    {
        for (int i = 0; i < maxVerts / cubeVerts; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

            cube.transform.position = new Vector3(Random.Range(-3f, 3f),
                                                  Random.Range(-3f, 3f),
                                                  Random.Range(-3f, 3f));

            cube.transform.Rotate(Random.Range(0, 360f),
                                  Random.Range(0, 360f),
                                  Random.Range(0, 360f));

            cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            cube.transform.SetParent(transform, true);

            cube.GetComponent<Renderer>().material = material;
        }
    }
}