using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class QuadRingMesh : MonoBehaviour
{
    [Range(0.01f, 2)]
    [SerializeField] float radiusInner;

    [Range(0.01f, 2)]
    [SerializeField] float thickness;

    [Range(3, 32)]
    [SerializeField] int angularSegments;

    Mesh mesh;

    float radiusOuter => radiusInner + thickness;
    int VertexCount => angularSegments * 2;

    private void OnDrawGizmosSelected() {

        GizmoScripts.DrawWireCircle(transform.position, transform.rotation, radiusInner, angularSegments);
        GizmoScripts.DrawWireCircle(transform.position, transform.rotation, radiusOuter, angularSegments);
    }

    private void Awake() {

        mesh = new Mesh();
        mesh.name = "QuadRing";
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    void Update() => GenerateMesh();

    void GenerateMesh() {
        // First clear
        mesh.Clear();

        int vCount = VertexCount;

        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();

        for (int i = 0; i < angularSegments; i++) {

            float t = i / (float)angularSegments;
            float angRad = t * 2 * Mathf.PI;

            Vector2 directionvector = MathScripts.GetUnitVectorByAngle(angRad);
            vertices.Add(directionvector * radiusOuter);
            vertices.Add(directionvector * radiusInner);
            normals.Add(Vector3.forward);
            normals.Add(Vector3.forward);

        }

        List<int> triangleindices = new List<int>();

        for (int i = 0; i < angularSegments; i++) {
            int rootindex = i * 2;

            int rootIndexInner = rootindex + 1;
            int rootIndexOuterNext = (rootindex + 2) % vCount;
            int rootIndexInnerNext = (rootindex + 3) % vCount;

            //triangle indices, count and turn is important.

            triangleindices.Add(rootindex);
            triangleindices.Add(rootIndexOuterNext); 
            triangleindices.Add(rootIndexInnerNext);

            triangleindices.Add(rootindex);
            triangleindices.Add(rootIndexInnerNext);
            triangleindices.Add(rootIndexInner);

        }

        mesh.SetVertices (vertices);
        mesh.SetTriangles (triangleindices, 0);
        mesh.SetNormals(normals);

    }
}

