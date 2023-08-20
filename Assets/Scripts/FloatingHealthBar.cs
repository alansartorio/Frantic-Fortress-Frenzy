using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;

    void Start()
    {
        var enemy = transform.parent.parent;
        var healthManager = enemy.GetComponent<HealthManager>();
        slider.value = (float)healthManager.health / healthManager.maxHealth;
        healthManager.onHealthChange.AddListener((healthManager) =>
        {
            slider.value = (float)healthManager.health / healthManager.maxHealth;
        });
    }

    void Update() {
        transform.rotation = Camera.main.transform.rotation;
        transform.position = target.position + offset;
    }
}
