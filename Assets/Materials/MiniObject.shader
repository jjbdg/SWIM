// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "MiniObject"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WIMPos ("Window Position", vector) = (0,0,0,0)
        _WIMRad ("Window Size", float) = .5
    }
    SubShader
    {
        // No culling or depth
        //Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 worldpos : TEXCOORD1;

                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert (appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v); //Insert
                UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldpos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            uniform float4 _WIMPos;
            uniform float _WIMRad;

            fixed4 frag(v2f i) : SV_Target
            {
                float4 diff = i.worldpos - _WIMPos;
                if (abs(diff.x) > _WIMRad || abs(diff.y) > _WIMRad || abs(diff.z) > _WIMRad) discard;
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
