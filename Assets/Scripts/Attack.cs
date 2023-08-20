using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public HealthManager targetHealth;
    public float attackCooldown = 0.5f;
    public float damage = 10f;
    private Timer timer;

    void Start()
    {
        timer = new Timer(attackCooldown);
        timer.onTick.AddListener(Execute);
    }

    void Update()
    {
        timer.Update(Time.deltaTime);
    }

    void Execute()
    {
        targetHealth.ApplyDamage(damage);
    }

    void OnDestroy()
    {
        if (timer != null)
            timer.onTick.RemoveListener(Execute);
    }
}
