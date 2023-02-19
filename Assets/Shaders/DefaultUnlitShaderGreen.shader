Shader "Unlit/DefaultUnlitShader"
{
    Properties // Input Data (excluding stuff supplied automatically by Unity like Meshes
    {
        // _MainTex ("Texture", 2D) = "white" {}
        // Default texture data deleted
        _Color("Color", Color) = (1,1,1,1)
    }
        SubShader // SubShaders contains the passes
    {
        Tags { "RenderType" = "Opaque" } // Render type
        LOD 100 // LOD levels based on distance

        Pass //blending mode
        {
            CGPROGRAM // Computer Graphics program is passed as HLSL code
            #pragma vertex vert // what function is vertex shader
            #pragma fragment frag // what function is fragment shader
            // make fog work
            // #pragma multi_compile_fog // fog added as default, cancelled for this example

            #include "UnityCG.cginc" // file containing Unity specific things - check later

            // sampler2D _MainTex;
            // float4 _MainTex_ST;
            // default texture data deleted
            float4 _Color;
            
            // automatically filled by Unity
            struct MeshData // mesh data per vertex
            {
                float4 vertex : POSITION; // vertex position, usually float4 in mesh data
                float3 normals : NORMAL;  // normals of objects
                // float4 tangent : TANGENT;  tangent information
                // float4 color : COLOR;  colour information, always float4
                float2 uv : TEXCOORD0; // uv0 coordinates (sample: diffuse/normal map textures)
                // float2 uv1 : TEXCOORD1; // uv1 coordinates (sample: lightmap coordinates)
                //semantics for uv refers to uv channels
            };

            // normals smoothly blend between vertices
            struct Interpolators // data that gets passed from vertex shader to fragment shader - aka interpolators
                
            {
                // float2 uv : TEXCOORD0; // different than uv channel above
                // UNITY_FOG_COORDS(1) // fog added as default, cancelled.
                float3 normal : TEXCOORD0; // normals
                float4 vertex : SV_POSITION; // clip space position of this vertex
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex); // converts local space to clip space, reason shader follows the object
                o.normal = UnityObjectToWorldNormal(v.normals); //just pass through
                return o;

                // o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // UNITY_TRANSFER_FOG(o,o.vertex);


            }

            // data types for HLSL
            // bool 0 1
            // int
            // float (32 bit float) workspace
            // half (16 bit float) usable
            // fixed (lower precision) -1 to 1
            // float4 -> half4 -> fixed4
            // float4x4 -> half4x4 (c#: Matrix 4x4)

            float4 frag(Interpolators i) : SV_Target
            {
                // float4 myvalue;
                // float2 otherValue = myValue.rg; // swizzling

                // sample the texture
                // fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                // UNITY_APPLY_FOG(i.fogCoord, col);

                // return _Color; // 4th is alpha channel, usually used in transparancy or to pass seperate data
                return float4 (i.normal, 1);
            }
            ENDCG

                // preference to complete operations in the vertex shader, fragment shaders takes all pixels into account, therefore works slower.
        }
    }
}
