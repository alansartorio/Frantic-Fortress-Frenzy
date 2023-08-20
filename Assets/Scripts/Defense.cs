using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class Defense : MonoBehaviour
{
    private TargetFinder targetFinder;
    private Attack attack;

    void Start()
    {
        targetFinder = GetComponent<TargetFinder>();
        attack = GetComponent<Attack>();

        targetFinder.onTargetEnter.AddListener(TargetEnter);
        targetFinder.onTargetExit.AddListener(TargetExit);
    }

    private void TargetEnter(ICollection<GameObject> enemies, GameObject addedEnemy)
    {
        if (attack.targetHealth == null) {
            attack.SetTarget(addedEnemy.GetComponent<HealthManager>());
        }
    }

    private void TargetExit(ICollection<GameObject> enemies, GameObject removedEnemy)
    {
        if (removedEnemy.GetComponent<HealthManager>() == attack.targetHealth) {
            var closestEnemy = enemies
                .OrderBy((enemy) => Vector3.Distance(enemy.transform.position, gameObject.transform.position))
                .FirstOrDefault();

            attack.SetTarget(closestEnemy?.GetComponent<HealthManager>());
        }
    }

    void OnDestroy()
    {
        if (targetFinder != null)
            targetFinder.onTargetEnter.RemoveListener(TargetEnter);
    }

    void Update()
    {
    }
}
