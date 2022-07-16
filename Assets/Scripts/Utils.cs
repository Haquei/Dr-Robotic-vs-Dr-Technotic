using System;
using UnityEngine;
using UnityEngine.AI;

public static class MathHelper
{
    public static float Mod(float x, float m)
    {
        float r = x % m;
        return r < 0 ? r + m : r;
    }
    public static Vector3 Mod(Vector3 v, float m)
    {
        return new Vector3(
            Mod(v.x, m),
            Mod(v.y, m),
            Mod(v.z, m)
        );
    }
}

public static class ParabolaHelper
{

    // Returns a lerp over a parabola over time t.
    public static Vector3 LerpParabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;
        var mid = Vector3.Lerp(start, end, t);
        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }
}

// Extensions

public static class NavMeshAgentExtensions
{
    public static bool IsAtDestination(this NavMeshAgent navMeshAgent)
    {
        if (navMeshAgent.pathPending) return false;
        if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance) return false;
        return !navMeshAgent.hasPath || Math.Abs(navMeshAgent.velocity.sqrMagnitude) < 0.01;
    }
}

public static class CameraExtensions
{

    public static Vector3 CastMouseOnFloor(this Camera camera)
    {
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        float distance = -ray.origin.y / ray.direction.y;
        return ray.GetPoint(distance);
    }

}