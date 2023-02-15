using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmoScripts
{
    public static void DrawWireCircle(Vector3 position, Quaternion rotation, float radius, int detail = 32) {

        Vector3[] points3D = new Vector3[detail];
        for (int i = 0; i < detail; i++) {

            float t = i / (float)detail;
            float angRad = t * 2 * Mathf.PI;

            Vector2 vector = MathScripts.GetUnitVectorByAngle(angRad)* radius;

            points3D[i] = position + rotation * vector;
        }

        for (int i = 0; i < detail-1; i++) {
            Gizmos.DrawLine(points3D[i], points3D[i + 1]);
        }

        Gizmos.DrawLine(points3D[detail - 1], points3D[0]);

    }
}
