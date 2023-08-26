using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wave
{
    private readonly int _enemyCount;
    private readonly GameObject[] _enemyTypes;

    public Wave(int enemyCount, GameObject[] enemyTypes)
    {
        _enemyCount = enemyCount;
        _enemyTypes = new GameObject[enemyCount];
        enemyTypes.CopyTo(_enemyTypes, 0);
    }

    public GameObject[] EnemyTypes => _enemyTypes;
    public int EnemyCount => _enemyCount;
}
