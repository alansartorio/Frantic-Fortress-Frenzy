using UnityEngine;

public class EnemyPath
{
    public Vector3[] waypoints;
    public static float waypointRadius = 2f;
    public GameObject target;

    public EnemyPath(Vector3[] waypoints, GameObject target)
    {
        this.waypoints = waypoints;
        this.target = target;
    }
}
