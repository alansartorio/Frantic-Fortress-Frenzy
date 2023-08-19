using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Health targetHealth;
    private float time = 0;
    public float attackCooldown = 0.5f;

    void Start()
    {
        targetHealth = GetComponent<Enemy>().target.GetComponent<Health>();
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > attackCooldown)
        {
            time = 0;
            Attack();
        }
    }

    void Attack()
    {
        targetHealth.ApplyDamage(10);
        if (targetHealth.Dead)
        {
            GetComponent<Enemy>().SetState(EnemyState.Idle);
        }
    }
}
