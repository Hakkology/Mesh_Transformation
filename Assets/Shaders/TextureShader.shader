Shader "Unlit/TextureShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // 3D Textures and CubeMaps also exist.
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct meshdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex; // Sampler must be added
            float4 _MainTex_ST; // optional _ST:Scale offset

            Interpolators vert (meshdata v)
            {
                Interpolators o;

                o.worldPos = mul(unity_ObjectToWorld, v.vertex); // transforming from local space to world space
                // depending on 4th argument, if 0, it will result in a vector or direction, if 1, it will result in a position.
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.uv.x += _Time.y * 0.1;
                return o;

            }

            float4 frag(Interpolators i) : SV_Target
            {

                float2 topDownProjection = i.worldPos.xz;

                // sample the texture
                float4 col = tex2D(_MainTex, topDownProjection);
                return col;
            }
            ENDCG
        }
    }
}
