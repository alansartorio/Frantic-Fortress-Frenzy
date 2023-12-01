using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileController : MonoBehaviour
{
    public string targetTag = "Enemy";
    public UnityEvent<LinkedList<GameObject>> onProjectileHitGround;
    
    private readonly LinkedList<GameObject> _targets = new();
    private bool executed = false;
    [SerializeField] public ParticleSystem explosionAnimation;


    private void Update()
    {
        // Check the rare few cases where the projectile misses the ground and falls into the void
        if (!executed && transform.position.y < 0f)
        {
            ExplodeAnimation();
            Destroy(gameObject, 0.1f);
            executed = true;
        }
    }

    private void ExplodeAnimation()
    {
        explosionAnimation.Play();
    }
    private void OnCollisionEnter(Collision other)
    {
        if(executed) return;
        var go = other.gameObject;
        if (go.name == "Floor")
        {
            onProjectileHitGround.Invoke(_targets);
            ExplodeAnimation();

            GameObject o;
            (o = gameObject).GetComponent<Rigidbody>().velocity = Vector3.zero;
            Destroy(o, 0.5f);
            executed = true;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var go = other.gameObject;
        if (go.CompareTag(targetTag))
        {
            _targets.AddFirst(go);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var go = other.gameObject;
        if (go.CompareTag(targetTag))
        {
            _targets.Remove(go);
        }
    }
}