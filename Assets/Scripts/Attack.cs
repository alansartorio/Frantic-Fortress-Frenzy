using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    public readonly LinkedList<HealthManager> targetsHealth = new();
    public bool attackOnStart = true;
    public float attackCooldown = 0.5f;
    public Health damage = new Health(10f,0f,0f);
    public bool resetOnTargetLost = true;
    private Timer _timer;
    public UnityEvent onAttack;
    public UnityEvent<ICollection<HealthManager>> onTargetChange;
    [SerializeField] public SimpleDamage[] prolongedDamages;
    private StackableProlongedDamage _stackableProlongedDamage;
    
    public enum TargetAction
    {
        Add,
        Remove,
        ClearAndAdd
    }

    private void Awake()
    {
        Dictionary<DamageType, float> damages = new();
        foreach (var simpleDamage in prolongedDamages)
        {
            damages.TryAdd(simpleDamage.type, simpleDamage.amount);
        }
        _stackableProlongedDamage = new StackableProlongedDamage(damages);
        _timer = new Timer(attackCooldown, attackOnStart);
        _timer.onTick.AddListener(Execute);
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }

    private void Execute()
    {
        // TODO: Improve efficiency.
        // When applying damage, the entity might die, so it will call UpdateTarget modifying targetsHealth.
        // That prints an error because you shouldn't modify the list while iterating.
        targetsHealth
            .ToList()
            .ForEach(t => t.ApplyDamage(damage, _stackableProlongedDamage));
        if (targetsHealth.Any())
            onAttack.Invoke();
    }

    public void UpdateTarget(IEnumerable<HealthManager> targets, TargetAction command)
    {
        switch (command)
        {
            case TargetAction.Add:
                targets.ForEach(t => targetsHealth.AddLast(t));
                break;
            case TargetAction.Remove:
                targets.ForEach(t => targetsHealth.Remove(t));
                break;
            case TargetAction.ClearAndAdd:
                targetsHealth.Clear();
                targetsHealth.AddRange(targets);
                break;
        }

        if (targetsHealth.Any())
        {
            _timer.Resume();
        }
        else
        {
            if (resetOnTargetLost)
            {
                _timer.Reset();
            }
            else
            {
                _timer.Hold();
            }
        }

        onTargetChange.Invoke(targetsHealth.AsReadOnlyCollection());
    }

    void OnDestroy()
    {
        _timer?.onTick.RemoveListener(Execute);
    }
}

[Serializable]
public struct SimpleDamage
{
    public DamageType type;
    public float amount;
}
