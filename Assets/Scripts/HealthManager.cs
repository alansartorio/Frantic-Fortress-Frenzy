using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    public float health;
    public UnityEvent<HealthManager> onTakeDamage;
    public UnityEvent<HealthManager> onDeath;

    public bool Dead
    {
        get => health <= 0;
        set => health = 0;
    }

    void Start()
    {
    }

    void Update()
    {
    }

    public void ApplyDamage(float damage)
    {
        if (health <= 0) return;
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            onDeath.Invoke(this);
        }
        else
        {
            onTakeDamage.Invoke(this);
        }
    }
}
