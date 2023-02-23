Shader "Unlit/Blending Modes"
{
    Properties
    {
        _ColorA("Color A", Color) = (1,1,1,1)
        _ColorB("Color B", Color) = (1,1,1,1)
        _WaveAmplitude("Wave Amplitude", Range(0,0.2)) = 0.1
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque"} 

        Pass
        {



        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        #include "UnityCG.cginc"

        #define TAU 6.28318530718 //pre-processor definition

        float4 _ColorA;
        float4 _ColorB;
        float _WaveAmplitude;

        struct meshdata
        {
            float4 vertex : POSITION;
            float2 uv0 : TEXCOORD0;
            float3 normals : NORMAL;
        };

        struct Interpolators
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION; //clip space position of each vertex
            float3 normal : TEXCOORD1;
        };

        float GetWave(float2 uv) {
            // ripple
            float2 uvsCentered = uv * 2 - 1;
            float radialDistance = length(uvsCentered);

            float wave = cos((radialDistance - _Time.y * 0.1) * TAU * 4) * 0.5 + 0.5;
            wave *= 1 - radialDistance;
            return wave;
        }

        Interpolators vert(meshdata v)
        {
            Interpolators o;

            // waves
            // float waveY = cos((v.uv0.y - _Time.y * 0.1) * TAU * 4);
            // float waveX = cos((v.uv0.x - _Time.y * 0.1) * TAU * 4);
            // v.vertex.y = waveX * waveY * _WaveAmplitude;

            v.vertex.y = GetWave(v.uv0);

            o.vertex = UnityObjectToClipPos(v.vertex);
            o.normal = UnityObjectToWorldNormal(v.normals);
            o.uv = v.uv0;
            return o;
        }

        float4 frag(Interpolators i) : SV_Target
        {
            float4 gradient = lerp(_ColorA, _ColorB, i.uv.y);
            return GetWave(i.uv) * gradient;
        }
        ENDCG
    }
    }
}
