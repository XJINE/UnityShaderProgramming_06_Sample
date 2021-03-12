using UnityEngine;

public class SetPropertyBlock : MonoBehaviour
{
    private void Start()
    {
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        materialPropertyBlock.SetColor("_Color", Color.red);
        GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);
    }
}