using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;
    public AudioClip damageAudioClip;
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
        audioSource.PlayOneShot(damageAudioClip, 0.7f);
        if (health <= 0)
        {
            health = 0;
        }
    }
}
