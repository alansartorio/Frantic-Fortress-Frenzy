using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class MultiDefense : Defense
{
    public override void TargetEnter(ICollection<GameObject> enemies, GameObject addedEnemy)
    {
        attack.UpdateTarget(UtilityEnumerable.Once(addedEnemy.GetComponent<HealthManager>()), Attack.TargetAction.Add);
    }

    public override void TargetExit(ICollection<GameObject> enemies, GameObject removedEnemy)
    {
        attack.UpdateTarget(UtilityEnumerable.Once(removedEnemy.GetComponent<HealthManager>()), Attack.TargetAction.Remove);
    }
}