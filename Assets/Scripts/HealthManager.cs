using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;
    public AudioClip damageAudioClip;
    public AudioClip deathAudioClip;
    private AudioSource audioSource;

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
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            audioSource.PlayOneShot(deathAudioClip, 0.7f);
        }
        else
        {
            audioSource.PlayOneShot(damageAudioClip, 0.7f);
        }
    }
}
