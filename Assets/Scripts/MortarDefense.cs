using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MortarDefense : Defense
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float timeToTarget = 3f;
    public Health damageAmount = new Health(10f,0,0);

    private bool initialized = false;
    private Vector3 _turretPosition;
    private Timer _attackTimer;
    private bool _isAttacking = false;
    public override void TargetEnter(ICollection<GameObject> enemies, GameObject addedEnemy)
    {
    }

    public override void TargetExit(ICollection<GameObject> enemies, GameObject removedEnemy)
    {
    }

    public override void GameUpdate(ICollection<GameObject> enemies, float deltaTime)
    {
        if (!initialized)
        {
            _turretPosition = gameObject.transform.position;
            _attackTimer = new Timer("Attack", attack.attackCooldown, false);
            _attackTimer.onTick.AddListener(() => { 
                _isAttacking = false;
                _attackTimer.Stop();
            });
            initialized = true;
        }
        _attackTimer.Update(deltaTime);
        if (_isAttacking)
        {
            return;
        }
        if (enemies.Count == 0)
        {
            return;
        }
        // Grab the first enemy in the list
        var target = enemies.First();
        // Predict the target's trajectory along the path to get the point to shoot at
        var targetPosition = PredictTargetPosition(target);
        Shoot(targetPosition);
    }

    private Vector3 PredictTargetPosition(GameObject target)
    {
        var enemy = target.GetComponent<Enemy>();
        var currentPosition = target.transform.position;
        var path = enemy.path.waypoints;
        var enemySpeed = enemy.chaseScript.speed;
        // Find the current waypoint
        var currentWaypointIndex = enemy.chaseScript.currentWaypointIndex - 1;
        var previousWaypoint = path[currentWaypointIndex];

        var position = enemy.path.target.transform.position;
        var nextWaypoint = currentWaypointIndex + 1 >= path.Length ? 
            (path[currentWaypointIndex] - position) * 0.2f + position : 
            path[currentWaypointIndex + 1];

        
        var enemyDirection = (nextWaypoint - previousWaypoint).normalized;
        var enemyVelocity = enemyDirection * enemySpeed;

        // We have to predict the target position at the time the projectile reaches it
        var targetPosition = currentPosition + enemyVelocity * timeToTarget;
        
        if(Vector3.Distance(targetPosition, currentPosition) > Vector3.Distance(nextWaypoint, currentPosition))
        {
            Vector3 turningWaypoint;
            if (currentWaypointIndex + 1 >= path.Length)
            {
                turningWaypoint = nextWaypoint;
            }
            else
            {
                if (currentWaypointIndex + 2 >= path.Length)
                {
                    var waypointTargetPosition = enemy.path.target.transform.position;
                    var adjustment = path[currentWaypointIndex] - waypointTargetPosition;
                    adjustment.Scale(new Vector3(0.1f, 0.1f, 0.1f));
                    turningWaypoint = waypointTargetPosition + adjustment;
                }
                else
                {
                    turningWaypoint = path[currentWaypointIndex + 2];
                }
            }

            var turnDirection = (turningWaypoint - nextWaypoint).normalized * 
                                        Vector3.Distance(targetPosition, nextWaypoint);
            targetPosition = nextWaypoint + turnDirection;            
        }
        
        return targetPosition;
    }

    private bool LineContains(Vector3 l1, Vector3 l2, Vector3 point)
    {
        var distanceToStart = Vector3.Distance(l1, point);
        var distanceToEnd = Vector3.Distance(l2, point);
        var lineLength = Vector3.Distance(l1, l2);

        return Math.Abs(distanceToStart + distanceToEnd - lineLength) < 0.001f;
    }

    private void Shoot(Vector3 target)
    {
        var position = new Vector3(_turretPosition.x, 0, _turretPosition.z);
        var distanceToTarget = Vector3.Distance(target, position);
        var targetDelta = target - position;
        
        var projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        var projectileController = projectile.GetComponentInChildren<ProjectileController>();

        if (projectileController != null)
        {
            projectileController.onProjectileHitGround.AddListener(OnProjectileHitGround);
        }
        var projectileRigidbody = projectile.GetComponentInChildren<Rigidbody>();
        var targetDirection = targetDelta.normalized;
        // var oblique = Quaternion.AngleAxis(-90, Vector3.up) * targetDirection;
        // var shootingDirection = (Quaternion.AngleAxis(shootingAngle, oblique) * targetDirection).normalized;

        var initialVelocity = new Vector3(
            targetDelta.x / timeToTarget,
            0.5f * Physics.gravity.magnitude * timeToTarget, 
            targetDelta.z / timeToTarget);
        projectileRigidbody.AddForce(initialVelocity, ForceMode.VelocityChange);
        
        _isAttacking = true;
        _attackTimer.Restart();
        
    }
    
    private void OnProjectileHitGround(LinkedList<GameObject> targets)
    {
        foreach (var target in targets.Where(target => target != null))
        {
            target.GetComponent<HealthManager>().ApplyDamage(damageAmount);
        }
    }

}
