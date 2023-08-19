using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public float speed = 2;
    private Transform targetTransform;


    void Start()
    {
        targetTransform = GetComponent<Enemy>().target.GetComponent<Transform>();
    }

    void Update()
    {
        var delta = targetTransform.position - transform.position;
        var direction = delta.normalized;
        transform.position += direction * speed * Time.deltaTime;
    }
}
