Shader "Sample/Struct"
{
    SubShader
    {
        Pass
        {
            CGPROGRAM

            #pragma target   5.0
            #pragma vertex   vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct particle
            {
                float2 position;
                float2 velocity;
                float  radius;
            };

            StructuredBuffer<particle> _ParticleBuffer;

            fixed4 frag(v2f_img input) : SV_Target
            {
                float4 color  = float4(0, 0, 0, 1);
                float  aspect = _ScreenParams.x / _ScreenParams.y;

                input.uv.x *= aspect;

                for (uint i = 0; i < _ParticleBuffer.Length; i++)
                {
                    particle p = _ParticleBuffer[i];

                    p.position.x *= aspect;

                    color += length(input.uv - p.position) < p.radius ? 1 : 0;
                }

                return color;
            }

            ENDCG
        }
    }
}