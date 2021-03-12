using UnityEngine;

public class Initialize : MonoBehaviour
{
    public Material material;
    public bool     combime;

    void Start()
    {
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.position = new Vector3(1.25f, 0, 0);
        quad.GetComponent<Renderer>().material = material;

        quad.isStatic = true;

        if (!combime)
        {
            return;
        }

        GameObject quadRoot = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quadRoot.transform.position = new Vector3(1.25f, 1.25f, 0);
        quadRoot.GetComponent<Renderer>().material = material;
        quadRoot.name = quadRoot.name + "(Root)";

        StaticBatchingUtility.Combine(new GameObject[] { quad, quadRoot }, quadRoot);
    }
}