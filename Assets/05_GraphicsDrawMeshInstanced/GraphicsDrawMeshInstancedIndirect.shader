Shader "Sample/GraphicsDrawMeshInstancedIndirect"
{
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }

        Pass
        {
            CGPROGRAM

            #pragma vertex   vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            StructuredBuffer<float4x4> _MatricesBuffer;

            v2f vert (appdata v, uint id : SV_InstanceID)
            {
                v2f o;
                v.vertex = mul(_MatricesBuffer[id], v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return float4(1, 1, 1, 1);
            }

            ENDCG
        }
    }
}