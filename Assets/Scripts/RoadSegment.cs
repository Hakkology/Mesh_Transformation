using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Security.Cryptography;

[RequireComponent(typeof(MeshFilter))]
public class RoadSegment : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField] float tTest = 0;

    [SerializeField]
    Transform[] controlPoints = new Transform[4];
    
    // Position get
    Vector3 GetPosition (int i) => controlPoints[i].position;
    

    private void OnDrawGizmos() {
        for (int i = 0; i < 4; i++) {
            Gizmos.DrawSphere(GetPosition(i), .05f);
        }

        Handles.DrawBezier(
            GetPosition(0), 
            GetPosition(3), 
            GetPosition(1), 
            GetPosition(2), Color.magenta, EditorGUIUtility.whiteTexture, 1f);

        Gizmos.color = Color.red;

        OrientedPoint testPoint = GetBezierOP(tTest);
        Handles.PositionHandle(testPoint.position, testPoint.rotation);

        float radius = .01f;
        void DrawPoint(Vector3 localPosition) => Gizmos.DrawSphere(testPoint.LocalToWorld(localPosition), radius);

        DrawPoint(Vector3.right * .2f);
        DrawPoint(Vector3.right * .1f);
        DrawPoint(Vector3.right * 0);
        DrawPoint(Vector3.right * -.1f);
        DrawPoint(Vector3.right * -.2f);
        DrawPoint(Vector3.up * .1f);
        DrawPoint(Vector3.up * .2f);
        
        Gizmos.color = Color.white;
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
}
