Shader "Sample/InstanceIdInVertex"
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
            //v2f vert(appdata v, uint id : SV_InstanceID)
            {
                UNITY_SETUP_INSTANCE_ID(v);

                uint id;

                #ifdef UNITY_INSTANCING_ENABLED
                id = UNITY_GET_INSTANCE_ID(v);
                id = unity_InstanceID;
                id = v.instanceID;
                #else
                id = 0;
                #endif

                v.vertex.xyz *= random3(id + 1);

                //v.vertex.xyz *= random3(unity_InstanceID + 1);

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                UNITY_TRANSFER_INSTANCE_ID(v, o);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return 1;
            }

            ENDCG
        }
    }
}