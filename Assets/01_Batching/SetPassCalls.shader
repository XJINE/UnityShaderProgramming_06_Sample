Shader "Sample/SetPassCalls"
{
    Properties{ }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }

        CGINCLUDE

        #include "UnityCG.cginc"

        struct v2f
        {
            float4 vertex : SV_POSITION;
        };

        v2f vert(appdata_base v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            return o;
        }

        ENDCG

        Pass
        {
            CGPROGRAM

            #pragma vertex   vert
            #pragma fragment frag

            fixed4 frag(v2f i) : SV_Target
            {
                return fixed4(1, 0, 0, 1);
            }

            ENDCG
        }

        Pass
        {
            Blend One One

            CGPROGRAM

            #pragma vertex   vert
            #pragma fragment frag

            fixed4 frag(v2f i) : SV_Target
            {
                return fixed4(0, 1, 1, 1);
            }

            ENDCG
        }
    }
}