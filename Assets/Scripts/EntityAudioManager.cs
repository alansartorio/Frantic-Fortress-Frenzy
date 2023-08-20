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
        if (deathAudioClip == null) return;
        var sound = new GameObject("Death Sound");
        sound.transform.SetPositionAndRotation(transform.position, transform.rotation);
        var source = sound.AddComponent<AudioSource>();
        source.clip = deathAudioClip;
        source.playOnAwake = false;
        source.volume = 0.7f;
        source.Play();
        Destroy(sound, 2f);
        // audioSource.PlayOneShot(deathAudioClip, 0.7f);
    }

    void PlayOnTakeDamageSound(HealthManager _)
    {
        if (damageAudioClip == null) return;
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
