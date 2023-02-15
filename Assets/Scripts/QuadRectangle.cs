using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadRectangle : MonoBehaviour {
    private void Awake() {

        Mesh QuadRectangleMesh = new Mesh();
        QuadRectangleMesh.name = "Procedural Quad";

        List<Vector3> points = new List<Vector3>(){

            new Vector3 (-1,1),
            new Vector3 (1,1),
            new Vector3 (-1,-1),
            new Vector3 (1,-1),

        };

        // 2,0,1 left hand thumb rule to determine the front side of the mesh.

        int[] triangleIndices = new int[]{
            1,0,2,
            3,1,2
        };

        List<Vector3> normals = new List<Vector3>(){

            new Vector3(0,0,1),
            new Vector3(0,0,1),
            new Vector3(0,0,1),
            new Vector3(0,0,1),

        };

        QuadRectangleMesh.SetVertices(points);
        QuadRectangleMesh.SetNormals(normals);
        QuadRectangleMesh.triangles = triangleIndices;
        //mesh.ReCalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = QuadRectangleMesh;


    }
}