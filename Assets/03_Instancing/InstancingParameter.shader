Shader "Sample/InstancingParameter"
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
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"
            #include "Assets/Common/Random.cginc"

            struct appdata
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID

                float4 vertex : POSITION;
            };

            struct v2f
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID

                float4 vertex : SV_POSITION;
            };

            UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_DEFINE_INSTANCED_PROP(int, _Index)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (appdata v)
            {
                UNITY_SETUP_INSTANCE_ID(v);

                int index = UNITY_ACCESS_INSTANCED_PROP(Props, _Index);
                v.vertex.xyz *= random3(index + 1);

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                UNITY_TRANSFER_INSTANCE_ID(v, o);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);

                int index = UNITY_ACCESS_INSTANCED_PROP(Props, _Index);

                return float4(random3(index).rgb, 1);
            }

            ENDCG
        }
    }
}