using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyState state;
    public GameObject target;
    public EnemyPath path;
    private EnemyChase chaseScript;
    private Attack attackScript;
    private EnemyIdle idleScript;

    void Start()
    {
        chaseScript = GetComponent<EnemyChase>();
        attackScript = GetComponent<Attack>();
        idleScript = GetComponent<EnemyIdle>();
        var targetHealth = target.GetComponent<HealthManager>();
        targetHealth.onDeath.AddListener((_) =>
        {
            SetState(EnemyState.Idle);
        });

        GetComponent<Attack>().onTargetChange.Invoke(null);
        GetComponent<HealthManager>().onDeath.AddListener((_) =>
        {
            Destroy(gameObject);
        });
        SetState(EnemyState.Walking);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == target && state != EnemyState.Idle)
        {
            SetState(EnemyState.Attacking);
            GetComponent<Attack>().onTargetChange.Invoke(target.GetComponent<HealthManager>());
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject == target && state != EnemyState.Idle)
        {
            SetState(EnemyState.Walking);
        }
    }

    void OnStateChange()
    {
        chaseScript.enabled = state == EnemyState.Walking;
        attackScript.enabled = state == EnemyState.Attacking;
        idleScript.enabled = state == EnemyState.Idle;
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
