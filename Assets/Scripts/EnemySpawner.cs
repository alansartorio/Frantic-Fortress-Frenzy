using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float globalTime = 0;
    private float time = 0;
    public float spawnInterval = 2f;
    public GameObject enemy;
    public GameObject target;

    void Start()
    {
        target.GetComponent<HealthManager>().onDeath.AddListener(() => {
            enabled = false;
        });
    }

    void Update()
    {
        time += Time.deltaTime;
        // globalTime += Time.deltaTime;
        // if (globalTime > 10f) {
        //     enabled = false;
        // }
        if (time > spawnInterval)
        {
            time = 0;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy() {
        Enemy newEnemy = Instantiate(enemy, transform.position, transform.rotation).GetComponent<Enemy>();
        newEnemy.target = target;
    }
}
