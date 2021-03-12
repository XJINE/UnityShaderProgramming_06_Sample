Shader "Sample/Position"
{
    SubShader
    {
        Pass
        {
            CGPROGRAM

            #pragma target   4.5//5.0
            #pragma vertex   vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            StructuredBuffer<float2> _Buffer;

            fixed4 frag(v2f_img input) : SV_Target
            {
                float4 color  = float4(0, 0, 0, 1);
                float  aspect = _ScreenParams.x / _ScreenParams.y;

                input.uv.x *= aspect;

                for (uint i = 0; i < _Buffer.Length; i++)
                {
                    float2 position = _Buffer[i] * aspect;

                    color += length(input.uv - position) < 0.1 ? 1 : 0;
                }

                return color;
            }

            ENDCG
        }
    }
}