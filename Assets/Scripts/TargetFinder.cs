using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Events;

public class TargetFinder : MonoBehaviour
{
    private readonly LinkedList<GameObject> targets = new();
    public string targetTag;
    public UnityEvent<ICollection<GameObject>, GameObject> onTargetEnter;
    public UnityEvent<ICollection<GameObject>, GameObject> onTargetExit;

    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            other.gameObject.GetComponent<HealthManager>().onDeath.AddListener(TargetDied);
            onTargetEnter.Invoke(targets.AsReadOnlyCollection(), other.gameObject);
            targets.AddLast(other.gameObject);
        }
    }

    void TargetDied(HealthManager healthManager) {
        RemoveTargetFromList(healthManager.gameObject);
    }

    void RemoveTargetFromList(GameObject enemy)
    {
        onTargetExit.Invoke(targets.AsReadOnlyCollection(), enemy);
        var node = targets.Find(enemy);
        node.Value.GetComponent<HealthManager>().onDeath.RemoveListener(TargetDied);
        targets.Remove(node);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            RemoveTargetFromList(other.gameObject);
        }
    }

    void Update()
    {
    }
}
