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
        slider.value = (float)healthManager.health.Hp / healthManager.maxHealth.Hp;
        healthManager.onHealthChange.AddListener((healthManager) =>
        {
            slider.value = (float)healthManager.health.Hp / healthManager.maxHealth.Hp;
        });
    }

    void Update() {
        transform.rotation = Camera.main.transform.rotation;
        transform.position = target.position + offset;
    }
}
