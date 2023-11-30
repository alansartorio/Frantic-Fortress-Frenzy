using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AreaDefense : Defense
{
    [SerializeField] private GameObject _barrel;
    [SerializeField] private double _coneAperture = Math.PI / 9; // 20 deg
    [SerializeField] private GameObject _targetEnemy = null;
    [SerializeField] private ParticleSystem _shootingSystem;

    public override void TargetEnter(ICollection<GameObject> enemies, GameObject addedEnemy)
    {
    }

    public override void TargetExit(ICollection<GameObject> enemies, GameObject removedEnemy)
    {
    }
    
    public override void GameUpdate(ICollection<GameObject> enemies)
    {
        if (enemies.Count == 0)
        {
            StopShooting();
            return;
        }
        // Compute center of mass with formula, assuming that every enemy has the same mass of 1
        var target = enemies.Aggregate(
            Vector3.zero,
            (sum, enemy) => sum + enemy.transform.position
            ) / enemies.Count;
        
        // Since the target can only be a HealthManager, then search for the nearest enemy from actual target
        var nearestEnemy = enemies.Aggregate((e1, e2) =>
        {
            var d1 = Vector3.Distance(target, e1.transform.position);
            var d2 = Vector3.Distance(target, e2.transform.position);
            return d1 < d2 ? e1 : e2;
        });
        _targetEnemy = nearestEnemy;
        
        // Once the turret has its target, add all enemies in cone area as well
        LinkedList<HealthManager> targets = new();
        targets.AddFirst(nearestEnemy.GetComponent<HealthManager>()); // The nearest enemy has to go first so it is targeted by the animator
        enemies.ForEach(enemy =>
        {
            if(enemy.Equals(nearestEnemy)) return;
            if (AngleToEnemy(enemy) < _coneAperture)
            {
                targets.AddLast(enemy.GetComponent<HealthManager>());
            }
        });

        if (targets.Count == 0)
        {
            StopShooting();
        }
        else
        {
            Shoot();
        }
        
        
        attack.UpdateTarget(targets, Attack.TargetAction.ClearAndAdd);
    }

    private void Shoot()
    {
        if (!_shootingSystem.isPlaying)
            _shootingSystem.Play();
        _shootingSystem.transform.LookAt(_targetEnemy.transform);
    }

    private void StopShooting()
    {
        if (_shootingSystem.isPlaying)
            _shootingSystem.Stop();
        attack.UpdateTarget(Enumerable.Empty<HealthManager>(), Attack.TargetAction.ClearAndAdd);
    }
    
    private float AngleToEnemy(GameObject enemy)
    {
        if (_targetEnemy == null)
        {
            throw new NullReferenceException("Target enemy has not been set");
        }
        var barrelPosition = _barrel.transform.position;
        var forwardDir = _targetEnemy.transform.position - barrelPosition;
        var targetDir = enemy.transform.position - barrelPosition;
        return Math.Abs(Vector3.Angle(forwardDir, targetDir));
    }
}