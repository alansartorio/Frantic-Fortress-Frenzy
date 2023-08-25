using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public float speed = 2;
    public Transform[] waypoints;
    public int currentWaypointIndex = 0;
    private Transform targetTransform;
    private Rigidbody targetRigidbody;
    public EnemyPath path;


    void Start()
    {
        waypoints = path.waypoints;
        targetTransform = path.target.GetComponent<Transform>();
        targetRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (currentWaypointIndex >= waypoints.Length)
        {
            MoveToPoint(targetTransform.position);
            return;
        }
        if (MoveToPoint(waypoints[currentWaypointIndex].position))
        {
            currentWaypointIndex += 1;
        }
    }

    private bool MoveToPoint(Vector3 point)
    {
        Vector3 delta3 = point - transform.position;
        var delta = new Vector2(delta3.x, delta3.z);

        // var direction = delta.normalized;

        var forward3 = transform.forward;
        var forward = new Vector2(forward3.x, forward3.z);

        var angle = Vector2.SignedAngle(forward, delta);

        targetRigidbody.rotation *= Quaternion.Euler(Vector3.up * (angle * -10 * Time.deltaTime));
        targetRigidbody.position += forward3 * (speed * Time.deltaTime);
        // targetRigidbody.AddForce(forward3 * speed, ForceMode.Force);
        // targetRigidbody.AddForce(new Vector3(direction.x, 0, direction.y) * speed, ForceMode.Force);

        return Vector3.Distance(point, transform.position) < EnemyPath.waypointRadius;
    }
}
