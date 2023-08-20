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
        var delta = targetTransform.position - transform.position;
        var direction = delta.normalized;
        targetRigidbody.AddForce(direction * speed, ForceMode.Force);
    }
}
