Shader "Sample/AppendConsumeBuffer"
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
            #include "Assets/Common/Random.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                uint   id     : TEXCOORD0;
            };

            struct particle
            {
                float3 position;
                float3 velocity;
                float  duration;
            };

            StructuredBuffer<particle> _ParticleBuffer;

            v2f vert(appdata v, uint instanceID : SV_InstanceID)
            {
                particle p = _ParticleBuffer[instanceID];

                v.vertex.xyz *= 0.1;

                v2f o;
                o.vertex = p.duration <= 0 ? 0 
                         : UnityObjectToClipPos(v.vertex + p.position);
                o.id     = instanceID;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return float4(random3(i.id), 1);
            }

            ENDCG
        }
    }
}