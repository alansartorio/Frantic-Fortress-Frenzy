using UnityEngine;

public class Wave
{
    private readonly int _enemyCount;
    private readonly GameObject[] _enemyTypes;

    public Wave(int enemyCount, GameObject[] enemyTypes)
    {
        _enemyCount = enemyCount;
        _enemyTypes = new GameObject[enemyTypes.Length];
        enemyTypes.CopyTo(_enemyTypes, 0);
    }

    public GameObject[] EnemyTypes => _enemyTypes;
    public int EnemyCount => _enemyCount;
}
