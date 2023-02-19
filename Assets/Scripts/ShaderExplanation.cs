using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderExplanation 
{
    /* 
     In Unity, shader structure (wholly called as ShaderLab) is as follows:

    .shader file format for shaders containing
        - properties
            .colours 
            .values
            .textures
                *These three properties are received from a Material - preconfigured properties to interact with shaders
            .mesh (imp.)
            .matrix 4x4 (transform data) (imp.)
        - sub-shader (multiple shaders per shader
            .pass (single or multiple)
                *Vertex Shader
                    -Mesh - vertices (foreach) - triangles
                    -Clipspace MVP Matrix
                    -3D 
                *Fragment Shader
                    -foreach Fragment = pixel
                    -colouring
                *Fragment (Pixel) Shader (HLSL Code)
    */

    /*
     * Unlit Shader Explanation
     
     */

    
}
