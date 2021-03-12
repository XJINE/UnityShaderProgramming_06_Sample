Shader "Sample/InstanceIdInFragment"
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

            v2f vert (appdata v)
            {
                UNITY_SETUP_INSTANCE_ID(v);

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                UNITY_TRANSFER_INSTANCE_ID(v, o);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            //fixed4 frag (v2f i, uint id : SV_InstanceID) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);

                uint id;

                #ifdef UNITY_INSTANCING_ENABLED
                id = UNITY_GET_INSTANCE_ID(i);
                //id = unity_InstanceID;
                //id = i.instanceID;
                #else
                id = 0;
                #endif

                return float4(random3(id.xxx + 1), 1);
            }

            ENDCG
        }
    }
}