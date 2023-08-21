using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnInterval = 2f;
    public int maxEnemies = 2;
    public GameObject enemy;
    public GameObject target;
    public GameObject pathObject;
    private Timer spawnTimer;
    private int _enemiesSpawned = 0;

    void Start()
    {
        spawnTimer = new Timer(spawnInterval, false);
        spawnTimer.Restart();
        spawnTimer.onTick.AddListener(SpawnEnemy);
        target.GetComponent<HealthManager>().onDeath.AddListener((_) =>
        {
            enabled = false;
        });
    }

    void Update()
    {
        spawnTimer.Update(Time.deltaTime);
    }

    private void SpawnEnemy()
    {
        if (_enemiesSpawned == maxEnemies) return;
        Enemy newEnemy = Instantiate(enemy, transform.position, transform.rotation).GetComponent<Enemy>();
        newEnemy.target = target;
        newEnemy.path = pathObject.GetComponent<EnemyPath>();
        _enemiesSpawned += 1;
    }

    void OnDestroy()
    {
        if (spawnTimer != null)
            spawnTimer.onTick.RemoveListener(SpawnEnemy);
    }
}
