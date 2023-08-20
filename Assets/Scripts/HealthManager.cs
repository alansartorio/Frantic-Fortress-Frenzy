using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    public float health;
    public AudioClip damageAudioClip;
    public AudioClip deathAudioClip;
    private AudioSource audioSource;
    public UnityEvent onDeath;

    public bool Dead
    {
        get => health <= 0;
        set => health = 0;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
    }

    public void ApplyDamage(float damage)
    {
        if (health <= 0) return;
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            onDeath.Invoke();
            audioSource.PlayOneShot(deathAudioClip, 0.7f);
        }
        else
        {
            audioSource.PlayOneShot(damageAudioClip, 0.7f);
        }
    }
}
