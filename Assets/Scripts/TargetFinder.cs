using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TargetFinder : MonoBehaviour
{
    private readonly LinkedList<GameObject> targets = new();
    public string targetTag;
    public UnityEvent<ICollection<GameObject>, GameObject> onTargetEnter;
    public UnityEvent<ICollection<GameObject>, GameObject> onTargetExit;
    public UnityEvent<ICollection<GameObject>> onGameUpdate;

    private bool IsEnemy(GameObject gameObject)
    {
        return gameObject.CompareTag(targetTag);
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsEnemy(other.gameObject))
        {
            other.gameObject.GetComponent<HealthManager>().onDeath.AddListener(TargetDied);
            targets.AddLast(other.gameObject);
            onTargetEnter.Invoke(targets.AsReadOnlyCollection(), other.gameObject);
        }
    }

    void TargetDied(HealthManager healthManager)
    {
        RemoveTargetFromList(healthManager.gameObject);
    }

    void RemoveTargetFromList(GameObject enemy)
    {
        var node = targets.Find(enemy);
        node.Value.GetComponent<HealthManager>().onDeath.RemoveListener(TargetDied);
        targets.Remove(node);
        onTargetExit.Invoke(targets.AsReadOnlyCollection(), enemy);
    }

    void OnTriggerExit(Collider other)
    {
        if (IsEnemy(other.gameObject))
        {
            RemoveTargetFromList(other.gameObject);
        }
    }

    void Update()
    {
        onGameUpdate.Invoke(targets.AsReadOnlyCollection());
    }
}
