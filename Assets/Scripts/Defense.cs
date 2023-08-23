using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public abstract class Defense : MonoBehaviour
{
    private TargetFinder targetFinder;
    protected Attack attack;
    public AudioClip shotSound;

    void Start()
    {
        attack = GetComponent<Attack>();
        targetFinder = GetComponent<TargetFinder>();

        targetFinder.onTargetEnter.AddListener(TargetEnter);
        targetFinder.onTargetExit.AddListener(TargetExit);

        var audio = GetComponent<AudioSource>();

        GetComponent<Attack>().onAttack.AddListener(() =>
        {
            audio.PlayOneShot(shotSound);
        });
    }

    public abstract void TargetEnter(ICollection<GameObject> enemies, GameObject addedEnemy);

    public abstract void TargetExit(ICollection<GameObject> enemies, GameObject removedEnemy);

    void OnDestroy()
    {
        if (targetFinder != null)
            targetFinder.onTargetEnter.RemoveListener(TargetEnter);
    }
}
