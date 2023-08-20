using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyState state;
    public GameObject target;
    private EnemyChase chaseScript;
    private EnemyAttack attackScript;

    void Start()
    {
        chaseScript = GetComponent<EnemyChase>();
        attackScript = GetComponent<EnemyAttack>();
        target.GetComponent<HealthManager>().onDeath.AddListener(() => {
            SetState(EnemyState.Idle);
        });
        SetState(EnemyState.Walking);
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.gameObject);
        if (collider.gameObject == target)
        {
            SetState(EnemyState.Attacking);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject == target)
        {
            SetState(EnemyState.Walking);
        }
    }

    void OnStateChange()
    {
        chaseScript.enabled = state == EnemyState.Walking;
        attackScript.enabled = state == EnemyState.Attacking;
    }

    public void SetState(EnemyState state)
    {
        this.state = state;
        OnStateChange();
    }

    void Update()
    {
    }
}
