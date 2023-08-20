using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnInterval = 2f;
    public GameObject enemy;
    public GameObject target;
    private Timer spawnTimer;

    void Start()
    {
        spawnTimer = new Timer(spawnInterval, false);
        spawnTimer.Restart();
        spawnTimer.onTick.AddListener(SpawnEnemy);
        target.GetComponent<HealthManager>().onDeath.AddListener((_) => {
            enabled = false;
        });
    }

    void Update()
    {
        spawnTimer.Update(Time.deltaTime);
    }

    private void SpawnEnemy() {
        Enemy newEnemy = Instantiate(enemy, transform.position, transform.rotation).GetComponent<Enemy>();
        newEnemy.target = target;
    }

    void OnDestroy() {
        if (spawnTimer != null)
            spawnTimer.onTick.RemoveListener(SpawnEnemy);
    }
}
