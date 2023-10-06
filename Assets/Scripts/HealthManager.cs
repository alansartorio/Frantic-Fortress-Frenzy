using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    public Health maxHealth;
    public Health health;
    public UnityEvent<HealthManager> onTakeDamage;
    public UnityEvent<HealthManager> onDeath;
    public UnityEvent<HealthManager> onHealthChange;
    private StackableProlongedDamage _stackableProlongedDamage = new();
    
    public bool Dead
    {
        get => health == Health.Zero;
        set => health = Health.Zero;
    }

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        _stackableProlongedDamage.Apply(this);
    }

    public void ApplyDamage(Health damage)
    {
        if (health == Health.Zero) return;
        health -= damage;
        if (health == Health.Zero)
        {
            onDeath.Invoke(this);
        }
        else
        {
            onTakeDamage.Invoke(this);
        }
        onHealthChange.Invoke(this);
    }

    public void ApplyDamage(Health flatDamage, StackableProlongedDamage stackedDamage)
    {
        _stackableProlongedDamage.StackDamage(stackedDamage);
        this.ApplyDamage(flatDamage);
    }
}

[Serializable]
public struct Health
{
    [SerializeField] private float _hp, _armor, _shield;
    private const double Tolerance = 0.1;
    private static readonly Health ZeroValue = new Health(0, 0, 0);

    public Health(float hp, float armor, float shield)
    {
        _hp = hp;
        _armor = armor;
        _shield = shield;
    }
    public float Hp
    {
        get => _hp;
        set => _hp = value > 0 ? value : 0;
    }

    public float Armor
    {
        get => _armor;
        set => _armor = value > 0 ? value : 0;
    }

    public float Shield
    {
        get => _shield;
        set => _shield = value > 0 ? value : 0;
    }

    public static Health Zero
    {
        get => ZeroValue;
        set => value = ZeroValue;
    }

    public static bool operator ==(Health first, Health second)
    {
        return Math.Abs(first.Hp - second.Hp) < Tolerance &&
               Math.Abs(first.Armor - second.Armor) < Tolerance &&
               Math.Abs(first.Shield - second.Shield) < Tolerance;
    }
        
    public static bool operator !=(Health first, Health second)
    {
        return !(first == second);
    }

    public static Health operator -(Health first, Health second)
    {
        var res = first;
        // TODO: should it reduce each type of health one by one, applying the (not yet implemented modifiers) or should it reduce all the health types at once, with stacking modifiers?
        res.Hp -= second.Hp;
        res.Armor -= second.Armor;
        res.Shield -= second.Shield;
        return res;
    }

    public static Health operator +(Health first, Health second)
    {
        var res = first;
        res._hp += second._hp;
        res._armor += second._armor;
        res._shield += second._shield;
        return res;
    }
        
    public bool Equals(Health other)
    {
        return _hp.Equals(other._hp) && _armor.Equals(other._armor) && _shield.Equals(other._shield);
    }

    public override bool Equals(object obj)
    {
        return obj is Health other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_hp, _armor, _shield);
    }
}
