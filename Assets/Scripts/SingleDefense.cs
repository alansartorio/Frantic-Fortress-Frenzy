using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SingleDefense : Defense
{
    public override void TargetEnter(ICollection<GameObject> enemies, GameObject addedEnemy)
    {
        if (attack.targetsHealth.Count != 0) return;

        attack.UpdateTarget(UtilityEnumerable.Once(addedEnemy.GetComponent<HealthManager>()), Attack.TargetAction.Add);
    }

    public override void TargetExit(ICollection<GameObject> enemies, GameObject removedEnemy)
    {
        if (!attack.targetsHealth.Contains(removedEnemy.GetComponent<HealthManager>())) return;

        var closestEnemy = enemies
            .Where(e => (bool)e)
            .OrderBy((enemy) => Vector3.Distance(enemy.transform.position, gameObject.transform.position))
            .FirstOrDefault();

        attack.UpdateTarget(UtilityEnumerable.Once(removedEnemy.GetComponent<HealthManager>()),
            Attack.TargetAction.Remove);
        if (closestEnemy != null)
            attack.UpdateTarget(UtilityEnumerable.Once(closestEnemy.GetComponent<HealthManager>()),
                Attack.TargetAction.Add);
    }
    
    public override void GameUpdate(ICollection<GameObject> enemies)
    {
    }
}