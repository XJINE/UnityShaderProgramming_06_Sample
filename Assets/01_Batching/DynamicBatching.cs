using UnityEngine;

public class DynamicBatching : MonoBehaviour
{
    public Material material;

    void Start()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Quad);
        cube.transform.position = new Vector3(1.25f, 0, 0);
        cube.GetComponent<Renderer>().material = material;
    }
}