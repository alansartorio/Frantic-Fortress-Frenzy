using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Defense : MonoBehaviour
{
    private TargetFinder _targetFinder;
    protected Attack attack;
    public AudioClip shotSound;

    void Start()
    {
        attack = GetComponent<Attack>();
        _targetFinder = GetComponent<TargetFinder>();

        _targetFinder.onTargetEnter.AddListener(TargetEnter);
        _targetFinder.onTargetExit.AddListener(TargetExit);
        _targetFinder.onGameUpdate.AddListener(GameUpdate);

        var audioSource = GetComponent<AudioSource>();

        GetComponent<Attack>().onAttack.AddListener(() =>
        {
            audioSource.PlayOneShot(shotSound);
        });
    }

    public abstract void TargetEnter(ICollection<GameObject> enemies, GameObject addedEnemy);

    public abstract void TargetExit(ICollection<GameObject> enemies, GameObject removedEnemy);

    public abstract void GameUpdate(ICollection<GameObject> enemies);

    void OnDestroy()
    {
        if (_targetFinder != null)
        {
            _targetFinder.onTargetEnter.RemoveListener(TargetEnter);
            _targetFinder.onTargetExit.RemoveListener(TargetExit);
            _targetFinder.onGameUpdate.RemoveListener(GameUpdate);
        }
    }
}
