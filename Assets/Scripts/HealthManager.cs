using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;

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
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Destroy(gameObject);
        }
    }
}
