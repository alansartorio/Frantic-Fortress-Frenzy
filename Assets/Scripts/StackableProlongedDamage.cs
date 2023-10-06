using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StackableProlongedDamage
{
    // The maximum amount of damage that can be applied per tick
    public static Dictionary<DamageType, float> damageCaps = new()
    {
        { DamageType.Burning, 0.5f },
    };

    public bool active = true;
    private readonly Dictionary<DamageType, float> _damageTypes;

    public StackableProlongedDamage(): this(new Dictionary<DamageType, float>()) { }
    
    public StackableProlongedDamage(Dictionary<DamageType, float> damage)
    {
        this._damageTypes = damage;
    }

    public void Apply(HealthManager healthManager)
    {
        // if(!active) return;
        bool isEmpty = true;
        for (var i = 0; i < _damageTypes.Keys.Count; i++)
        {
            var type = _damageTypes.Keys.ElementAt(i);
            var amount = _damageTypes[type];
            if (IsZero(amount)) continue;
            
            var amountToApply = Math.Min(amount, damageCaps[type]);
            _damageTypes[type] -= amountToApply;
            if(!IsZero(amount)) isEmpty = false;
            var dmg = DamageUtils.AsHealth(amountToApply, type);
            Debug.Log("Applying " + dmg.Hp + " damage to " + healthManager.gameObject.name);
            healthManager.ApplyDamage(dmg);
        }
        
        if (isEmpty)
        {
            active = false;
        }
    }

    public float getFireDamage()
    {
        _damageTypes.TryGetValue(DamageType.Burning, out var value);
        return value;
    }

    public void StackDamage(StackableProlongedDamage other)
    {
        foreach (var type in other._damageTypes.Keys)
        {
            var amount = other._damageTypes[type];
            if (this._damageTypes.TryAdd(type, amount)) continue;
            this._damageTypes[type] += amount;
        }

        active = true;
    }

    private bool IsZero(float value)
    {
        return Math.Abs(value) < 0.001;
    }
}

