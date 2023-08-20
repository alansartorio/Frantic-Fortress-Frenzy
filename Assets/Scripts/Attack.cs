using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    public HealthManager targetHealth;
    public bool attackOnStart = true;
    public float attackCooldown = 0.5f;
    public float damage = 10f;
    public bool resetOnTargetLost = true;
    private Timer timer;
    public UnityEvent onAttack;

    void Awake()
    {
        timer = new Timer(attackCooldown, attackOnStart);
        timer.onTick.AddListener(Execute);
    }

    void Update()
    {
        timer.Update(Time.deltaTime);
    }

    void Execute()
    {
        targetHealth?.ApplyDamage(damage);
        onAttack.Invoke();
    }

    public void SetTarget(HealthManager target)
    {
        targetHealth = target;
        if (target == null)
        {
            if (resetOnTargetLost)
            {
                timer.Reset();
            }
            else
            {
                timer.Hold();
            }
        }
        else
        {
            timer.Resume();
        }
    }

    void OnDestroy()
    {
        if (timer != null)
            timer.onTick.RemoveListener(Execute);
    }
}
