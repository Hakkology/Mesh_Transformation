using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public struct OrientedPoint
{
    public Vector3 position;
    public Quaternion rotation;

    public OrientedPoint(Vector3 _position, Quaternion _rotation) {

        this.position = _position;
        this.rotation = _rotation;
    }
    public OrientedPoint(Vector3 _position, Vector3 _forward) {

        this.position = _position;
        this.rotation = Quaternion.LookRotation(_forward);
    }

    // local position of the point
    public Vector3 LocalToWorldPosition(Vector3 localSpacePosition) {

        return position + rotation * localSpacePosition;
    }

    // local rotation of the point
    public Vector3 LocalToWorldVector (Vector3 localSpacePosition) {
        return rotation * localSpacePosition;
    }
}
