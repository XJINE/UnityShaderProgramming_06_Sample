using UnityEngine;

public class InstancingParameter : MonoBehaviour
{
    public Material material;
    public int      count = 10000;

    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

            cube.transform.position = new Vector3(Random.Range(-3f, 3f),
                                                  Random.Range(-3f, 3f),
                                                  Random.Range(-3f, 3f));

            cube.transform.Rotate(Random.Range(0, 360f),
                                  Random.Range(0, 360f),
                                  Random.Range(0, 360f));

            cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            Renderer renderer = cube.GetComponent<Renderer>();
            renderer.material = material;

            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetInt("_Index", i);
            renderer.SetPropertyBlock(propertyBlock);
        }
    }
}