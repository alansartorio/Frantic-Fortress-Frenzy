using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class FireTowerAnimator : MonoBehaviour
{
    private Attack attack;
    private GameObject target;
    public GameObject barrel;

    void Start()
    {
        attack = GetComponentInParent<Attack>();
        if (attack)
            attack.onTargetChange.AddListener((targets) => target = targets.Any() ? targets.First().gameObject : null);
    }

    void Update()
    {
        if (target)
        {
            barrel.transform.LookAt(target.transform);
        }
    }
}