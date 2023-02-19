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

    public int VertexCount => vertices.Length;
    public int LineCount => lineindices.Length;

    public float CalcUSpan() {

        float distance = 0;
        for (int i = 0; i < LineCount; i+=2) {

            Vector2 uA = vertices[lineindices[i]].points;
            Vector2 uB = vertices[lineindices[i+1]].points;
            distance += Vector3.Distance(uA, uB);

        }

        return distance;    
    }


}
