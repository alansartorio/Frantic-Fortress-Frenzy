using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyState state;
    public EnemyPath path;
    private EnemyChase chaseScript;
    private Attack attackScript;
    private EnemyIdle idleScript;
    private StackableProlongedDamage _stackableProlongedDamage= new ();
    [SerializeField] private int _unitScore = 10;

    void Start()
    {
        chaseScript = GetComponent<EnemyChase>();
        chaseScript.path = path;
        attackScript = GetComponent<Attack>();
        idleScript = GetComponent<EnemyIdle>();
        var targetHealth = path.target.GetComponent<HealthManager>();
        targetHealth.onDeath.AddListener((_) =>
        {
            SetState(EnemyState.Idle);
        });

        GetComponent<HealthManager>().onDeath.AddListener((_) =>
        {
            Destroy(gameObject);
        });
        SetState(EnemyState.Walking);
    }
    
    public int GetScore()
    {
        return _unitScore;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == path.target && state != EnemyState.Idle)
        {
            SetState(EnemyState.Attacking);
            GetComponent<Attack>().UpdateTarget(UtilityEnumerable.Once(path.target.GetComponent<HealthManager>()), Attack.TargetAction.ClearAndAdd);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject == path.target && state != EnemyState.Idle)
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
