using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static float GetPlanarAngleTo(this Transform callerTransform, Vector3 point)
    {
        return Vector2.SignedAngle(new Vector2(callerTransform.forward.x, callerTransform.forward.z), new Vector2(point.x, point.z) - new Vector2(callerTransform.position.x, callerTransform.position.z));
    }
}
