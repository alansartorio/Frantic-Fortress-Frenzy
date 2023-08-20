using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAudioManager : MonoBehaviour
{
    public AudioClip damageAudioClip;
    public AudioClip deathAudioClip;
    private AudioSource audioSource;
    private HealthManager healthManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        healthManager = GetComponent<HealthManager>();
        healthManager.onDeath.AddListener(PlayOnDeathSound);
        healthManager.onTakeDamage.AddListener(PlayOnTakeDamageSound);
    }

    void Update()
    {

    }

    void PlayOnDeathSound(HealthManager _)
    {
        audioSource.PlayOneShot(deathAudioClip, 0.7f);
    }

    void PlayOnTakeDamageSound(HealthManager _)
    {
        audioSource.PlayOneShot(damageAudioClip, 0.7f);
    }

    void OnDestroy()
    {
        if (healthManager != null)
        {
            healthManager.onDeath.RemoveListener(PlayOnDeathSound);
            healthManager.onTakeDamage.RemoveListener(PlayOnTakeDamageSound);
        }
    }
}
