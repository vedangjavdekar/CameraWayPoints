using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWayPoints : MonoBehaviour
{
    public bool drawGizmos;
    public List<WayPoint> points = new List<WayPoint>();

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;
        for ( int i=0;i< points.Count;i++)
        {
            if (i > 0)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(points[i - 1].position, points[i].position);
            }
            Gizmos.color = points[i].color;
            Gizmos.DrawSphere(points[i].position, 0.1f);
            Gizmos.DrawRay(points[i].position, 0.5f* ToSpherical(points[i].angles));

        }
    }

    private Vector3 ToSpherical(Vector3 angles)
    {
        float yaw = angles.y * Mathf.Deg2Rad;
        float pitch = -angles.x * Mathf.Deg2Rad;

        float z = Mathf.Cos(yaw) * Mathf.Cos(pitch);
        float x = Mathf.Sin(yaw) * Mathf.Cos(pitch);
        float y = Mathf.Sin(pitch);

        return new Vector3(x, y, z).normalized;
    }
}

[System.Serializable]
public struct WayPoint
{
    public Color color;
    public Vector3 position;
    public Vector3 angles;
}