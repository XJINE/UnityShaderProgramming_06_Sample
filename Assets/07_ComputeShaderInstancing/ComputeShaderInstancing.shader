Shader "Sample/ComputeShaderInstancing"
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
                float4 color  : TEXCOORD1;
            };

            struct particle
            {
                float3 position;
                float3 velocity;
                float4 color;
            };

            StructuredBuffer<particle> _ParticleBuffer;

            v2f vert (appdata v, uint instanceID : SV_InstanceID)
            {
                particle p = _ParticleBuffer[instanceID];

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex + p.position);
                o.color  = p.color;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return i.color;
            }

            ENDCG
        }
    }
}