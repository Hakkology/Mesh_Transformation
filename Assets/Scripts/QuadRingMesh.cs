using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class QuadRingMesh : MonoBehaviour
{
    public enum UVProjection {
        AngularRadial,
        ProjectZ
    }

    [Range(0.01f, 2)]
    [SerializeField] float radiusInner;

    [Range(0.01f, 2)]
    [SerializeField] float thickness;

    [Range(3, 128)]
    [SerializeField] int angularSegments;

    [SerializeField]
    UVProjection uVProjection = QuadRingMesh.UVProjection.AngularRadial;

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
        List<Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < angularSegments+1; i++) {

            float t = i / (float)angularSegments;
            float angRad = t * 2 * Mathf.PI;

            Vector2 directionvector = MathScripts.GetUnitVectorByAngle(angRad);

            //Vector3 zOffset = Vector3.forward * Mathf.Cos(angRad *4);

            vertices.Add((Vector3)(directionvector * radiusOuter) );
            vertices.Add((Vector3)(directionvector * radiusInner) );
            normals.Add(Vector3.forward);
            normals.Add(Vector3.forward);

            switch (uVProjection) {
                case UVProjection.AngularRadial:
                    uvs.Add(new Vector2(t, 1));
                    uvs.Add(new Vector2(t, 0));
                    break;
                case UVProjection.ProjectZ:
                    uvs.Add(directionvector * 0.5f + Vector2.one * 0.5f);
                    uvs.Add(directionvector * (radiusInner / radiusOuter) * 0.5f + Vector2.one * 0.5f);
                    break;
            }

            //uvs.Add(new Vector2(t, 1));
            //uvs.Add(new Vector2(t, 0));

        }

        List<int> triangleindices = new List<int>();

        for (int i = 0; i < angularSegments; i++) {
            int rootindex = i * 2;

            int rootIndexInner = rootindex + 1;
            int rootIndexOuterNext = rootindex + 2;
            int rootIndexInnerNext = rootindex + 3;

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
        mesh.SetUVs(0, uvs);

    }
}

