using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class RoadSegment : MonoBehaviour
{
    [SerializeField] Mesh2D crossSection;

    [Range(0f, 32f)]
    [SerializeField] int crossSectionCount = 8;

    [Range(0.0f, 1.0f)]
    [SerializeField] float tTest = 0;
    [SerializeField]
    Transform[] controlPoints = new Transform[4];
    // Position get
    Vector3 GetPosition (int i) => controlPoints[i].position;

    Mesh mesh;

    private void Awake() {
        mesh = new Mesh();
        mesh.name = "Segment";
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    void Update() => GenerateMesh();

    void GenerateMesh() {

        float uSpan = crossSection.CalcUSpan();
        mesh.Clear();

        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector3> uvs = new List<Vector3>();

        // Generating Vertices, Normals, UVs
        for (int cSection = 0; cSection < crossSectionCount + 1; cSection++) {

            // First generate oriented points for each cross section. Amount of cross section is provided as detail and interchangable, which also makes t interchangable between ranges of 0 and 1, which also increases the detail.
            float t = cSection / (crossSectionCount-1f);
            OrientedPoint orientedPoint = GetBezierOP(t);

            for (int i = 0; i < crossSection.vertices.Length; i++) {
                vertices.Add(orientedPoint.LocalToWorldPosition(crossSection.vertices[i].points));
                normals.Add(orientedPoint.LocalToWorldVector(crossSection.vertices[i].normals));
                uvs.Add(new Vector2(crossSection.vertices[i].uvs * GetApproxLength() / uSpan , t));
            }
        }

        List<int> triangleIndices = new List<int>();

        // Generating Triangles
        // -1 is implemented because we have no desire to include any more ring after the last cSection.
        for (int cSection = 0; cSection < crossSectionCount-1; cSection++) {

            int rootIndexCurrent = cSection * crossSection.VertexCount;
            int rootIndexNext = (cSection + 1) * crossSection.VertexCount;

            // iterating through all the lines
            for (int line = 0; line < crossSection.LineCount; line+=2) {

                int lineIndexA = crossSection.lineindices[line];
                int lineIndexB = crossSection.lineindices[line+1];

                int currentA = rootIndexCurrent + lineIndexA;
                int currentB = rootIndexCurrent + lineIndexB;
                int nextA = rootIndexNext + lineIndexA;
                int nextB = rootIndexNext + lineIndexB;

                // forming triangles
                triangleIndices.Add(currentA);
                triangleIndices.Add(nextA);
                triangleIndices.Add(nextB);

                triangleIndices.Add(currentA);
                triangleIndices.Add(nextB);
                triangleIndices.Add(currentB);

            }
        }

        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangleIndices, 0);
    }

    private void OnDrawGizmos() {
        for (int i = 0; i < 4; i++) {
            Gizmos.DrawSphere(GetPosition(i), .05f);
        }

        Handles.DrawBezier(
            GetPosition(0), 
            GetPosition(3), 
            GetPosition(1), 
            GetPosition(2), Color.magenta, EditorGUIUtility.whiteTexture, 1f);

        //Gizmos.color = Color.red;

        OrientedPoint testPoint = GetBezierOP(tTest);
        Handles.PositionHandle(testPoint.position, testPoint.rotation);

        float radius = .1f;
        void DrawPoint(Vector3 localPosition) => Gizmos.DrawSphere(testPoint.LocalToWorldPosition(localPosition), radius);

        Vector3[] localVertices = crossSection.vertices.Select(v => testPoint.LocalToWorldPosition(v.points)).ToArray();
        Gizmos.color = Color.grey;

        // iterate through all the vertices
        for (int i = 0; i < crossSection.lineindices.Length; i+=2) {
            
            Vector3 a = localVertices[crossSection.lineindices[i]];
            Vector3 b = localVertices[crossSection.lineindices[i+1]];
            Gizmos.DrawLine(a, b);
            //DrawPoint(crossSection.vertices[i].points);
        }

        //DrawPoint(Vector3.right * .2f);
        //DrawPoint(Vector3.right * .1f);
        //DrawPoint(Vector3.right * 0);
        //DrawPoint(Vector3.right * -.1f);
        //DrawPoint(Vector3.right * -.2f);
        //DrawPoint(Vector3.up * .1f);
        //DrawPoint(Vector3.up * .2f);
        
        //Gizmos.color = Color.white;
    }

    OrientedPoint GetBezierOP( float t) {
        Vector3 p0 = GetPosition(0);
        Vector3 p1 = GetPosition(1);
        Vector3 p2 = GetPosition(2);
        Vector3 p3 = GetPosition(3);

        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        Vector3 position = Vector3.Lerp(d, e, t);
        Vector3 tangent = (e - d).normalized;
        return new OrientedPoint(position, tangent);
    }

    // Approximate length of a bezier curve
    float GetApproxLength (int precision = 8) {
        Vector3[] points = new Vector3[(int)precision];

        for (int i = 0; i < precision; i++) {

            float t = i / (precision-1);
            points[i] = GetBezierOP(t).position;

        }

        float distance = 0;
        for (int i = 0; i < precision - 1; i++) {

            Vector3 a = points[i];
            Vector3 b = points[i+1];
            distance += Vector3.Distance(a, b);

        }

        return distance;
    }
}
