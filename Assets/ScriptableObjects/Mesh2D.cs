using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class Mesh2D : ScriptableObject
{
    [System.Serializable]
    public class Vertex {
        public Vector2 points;
        public Vector2 normals;
        public float uvs; // in this case no V

        //vertex color
        //bitangent etc could be added
    }

    public Vertex[] vertices;
    public int[] lineindices;


}
