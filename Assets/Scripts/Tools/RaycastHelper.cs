using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class used to visual types of casts. Use inside Gizmo methods.
/// </summary>
public static class RaycastHelper 
{
    public static void DrawRayCast(Vector3 origin, Vector3 direction, float distance) {
        Gizmos.DrawLine(origin, origin + direction * distance);
    }

    public static void DrawSphereCast(Vector3 origin, float radius, Vector3 direction, float distance, bool wireframe = true) {
        Gizmos.DrawLine(origin, origin + direction * distance);
        if (wireframe)
            Gizmos.DrawWireSphere(origin + direction * distance, radius);
        else
            Gizmos.DrawSphere(origin + direction * distance, radius);
    }

    public static void DrawBoxCast(Vector3 origin, Vector3 extents, Vector3 direction, float distance, bool wireframe = true) {
        Gizmos.DrawLine(origin, origin + direction * distance);
        if (wireframe)
            Gizmos.DrawWireCube(origin + direction * distance, extents);
        else
            Gizmos.DrawCube(origin + direction * distance, extents);
    }

    public static void DrawCapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float distance, bool wireframe = true) {
        var midPoint = Vector3.Lerp(point1, point2, 0.5f);
        Gizmos.DrawLine(midPoint, midPoint + direction * distance);
        if (wireframe) {
            Gizmos.DrawWireSphere(point1 + direction * distance, radius);
            Gizmos.DrawWireSphere(point2 + direction * distance, radius);
        } else {
            Gizmos.DrawSphere(point1 + direction * distance, radius);
            Gizmos.DrawSphere(point2 + direction * distance, radius);
        }
    }
}
