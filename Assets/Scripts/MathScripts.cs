using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathScripts
{
    public static Vector2 GetUnitVectorByAngle(float angRad) {

        return new Vector2(
            Mathf.Cos(angRad),
            Mathf.Sin(angRad)
            );
    }
}
