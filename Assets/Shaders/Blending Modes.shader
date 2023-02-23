Shader "Unlit/Blending Modes"
{
    Properties
    {
        _ColorA("Color A", Color) = (1,1,1,1)
        _ColorB("Color B", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" //tags to inform the render pipeline of what type this is
               "Queue" = "Transparent" 
        } //tags to change the render order

        Pass
        {

            Cull Off //Culling means removal, *Cull Back, *Cull Front 
            ZWrite Off //Now no depth buffer, explained below
            //ZTest LEqual //LEqual default value //LEqual draws if in frontal side, GEqual draws if in back side.
            Blend One One // additive
            //Blend DstColor Zero // multiply
            //Depth buffer : Some shaders have a depth buffer at a value between 0 and 1.
            //Optimization to check if an object is behind another object.

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define TAU 6.28318530718 //pre-processor definition

            float4 _ColorA;
            float4 _ColorB;

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

            Interpolators vert (meshdata v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normals);
                o.uv = v.uv0;
                return o;
            }

            float4 frag(Interpolators i) : SV_Target
            {
                //return i.uv.y;

                float xOffset = cos (i.uv.x * TAU * 8) * 0.01;
                float t = cos ((i.uv.y + xOffset - _Time.y * 0.1) * TAU * 4) * 0.5 + 0.5;
                t *= 1-i.uv.y; // fade out the ring around the cylinder

                float topBottomRemover = (abs(i.normal.y) < 0.999);
                float waves = t * topBottomRemover;

                float4 gradient = lerp(_ColorA, _ColorB, i.uv.y);
                float4 ringWave = waves * gradient;
                return ringWave; // removing top and bottom layers
            }
            ENDCG
        }
    }
}
