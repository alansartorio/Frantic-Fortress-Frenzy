using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireTowerAnimator : MonoBehaviour
{
    private Attack attack;
    private GameObject target;
    public GameObject barrel;
    
    void Start()
    {
        attack = GetComponent<Attack>();
        attack.onTargetChange.AddListener((newTarget) => target = newTarget?.gameObject);
    }

    void Update()
    {
        if (target != null) {
            barrel.transform.LookAt(attack.targetHealth.transform);
        }
    }
}
