using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Health targetHealth;

    void Start()
    {
        targetHealth = GetComponent<Enemy>().target.GetComponent<Health>();
    }

    void Update()
    {
        targetHealth.ApplyDamage(10);
        if (targetHealth.Dead) {
            GetComponent<Enemy>().SetState(EnemyState.Idle);
        }
    }
}
