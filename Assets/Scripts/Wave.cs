using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wave
{
    private readonly int _maxEnemies;
    private int _spawnCount = 0;
    private bool _active = true;
    private readonly GameObject[] _enemies;

    public Wave(int maxEnemies, GameObject[] enemies)
    {
        _maxEnemies = maxEnemies;
        _enemies = new GameObject[maxEnemies];
        for (int i = 0; i < maxEnemies; i++)
        {
            _enemies[i] = enemies[Random.Range(0, enemies.Length)];
        }
    }

    public GameObject GetNextEnemy()
    {
        if (!_active || _spawnCount == _maxEnemies) return null;
        return _enemies[_spawnCount++];
    }

    public bool IsActive()
    {
        return _active;
    }

    public void Deactivate()
    {
        _active = false;
    }

    public void Activate()
    {
        _active = true;
    }
}
