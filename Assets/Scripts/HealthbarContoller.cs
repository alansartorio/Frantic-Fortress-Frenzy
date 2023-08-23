using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Transform target;

    private Image _hpBarImage;
    private Image _armorBarImage;
    private Image _shieldBarImage;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Start()
    {
        Transform hbContent = transform.Find("Canvas").Find("Content");
        _hpBarImage = hbContent.Find("Hp Bar").GetComponent<Image>();
        _armorBarImage = hbContent.Find("Armor Bar").GetComponent<Image>();
        _shieldBarImage = hbContent.Find("Shield Bar").GetComponent<Image>();
        var enemy = transform.parent;
        var healthManager = enemy.GetComponent<HealthManager>();
        SetFillValues(healthManager);
        healthManager.onHealthChange.AddListener(SetFillValues);
    }

    void Update() {
        transform.rotation = mainCamera.transform.rotation;
        // transform.position = target.position + offset;
    }

    private void SetFillValues(HealthManager healthManager)
    {
        var totalHealth = healthManager.maxHealth.Hp + healthManager.maxHealth.Armor + healthManager.maxHealth.Shield;
        _hpBarImage.fillAmount = (float)healthManager.health.Hp / totalHealth;
        _armorBarImage.fillAmount = _hpBarImage.fillAmount + (float)healthManager.health.Armor / totalHealth;
        _shieldBarImage.fillAmount = _armorBarImage.fillAmount + (float)healthManager.health.Shield / totalHealth;
    }
}
