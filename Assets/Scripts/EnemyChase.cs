using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public float speed = 2;
    private Transform targetTransform;
    private Rigidbody targetRigidbody;


    void Start()
    {
        targetTransform = GetComponent<Enemy>().target.GetComponent<Transform>();
        targetRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 delta3 = targetTransform.position - transform.position;
        var delta = new Vector2(delta3.x, delta3.z);

        var direction = delta.normalized;

        var forward3 = transform.forward;
        var forward = new Vector2(forward3.x, forward3.z);

        var angle = Vector2.SignedAngle(forward, delta);

        targetRigidbody.AddTorque(Vector3.up * angle * -0.01f);
        targetRigidbody.AddForce(new Vector3(direction.x, 0, direction.y) * speed, ForceMode.Force);
    }
}
