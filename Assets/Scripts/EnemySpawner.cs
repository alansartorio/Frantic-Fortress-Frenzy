using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private static readonly float SpawnInterval = 1f;
    public EnemyPath path;
    private Timer _spawnTimer;

    /**
     * TODO: maybe load several waves in advance to make it better performant
     */
    [SerializeField] private Wave _currentWave;

    private List<GameObject> _enemiesLeft = new();
    private int _enemiesKilled = 0;
    private readonly UnityEvent _allEnemiesDead = new();
    private readonly UnityEvent _gameOver = new();
    private readonly UnityEvent<Enemy> _enemyKilled = new();
    [SerializeField] private TMP_Text _costText;

    void Start()
    {
        _spawnTimer = new Timer("Spawn", SpawnInterval, true);
        _spawnTimer.Stop();
        _spawnTimer.onTick.AddListener(SpawnNextEnemy);

        var gameDirector = FindObjectOfType<GameDirector>();
        gameDirector.OnTerrainExpand.AddListener(SetCost);
        SetCost(gameDirector.GetExpandCost());
        
        gameDirector.RegisterSpawn(new GameDirector.SpawnerInfo()
        {
            enemiesWiped = _allEnemiesDead,
            enemyKilled = _enemyKilled,
            onGameOver = () => _gameOver.Invoke(),
            onNewWave = SpawnWave
        });
    }

    void SetCost(int cost)
    {
        _costText.SetText($"${cost}");
    }

    void Update()
    {
        _spawnTimer.Update(Time.deltaTime);
    }

    private void SpawnWave(Wave wave)
    {
        _currentWave = wave;
        _enemiesLeft = wave.Enemies.ToList();
        _enemiesKilled = 0;
        _spawnTimer.Restart();
    }

    private void SpawnNextEnemy()
    {
        if (!_enemiesLeft.Any()) return;
        SpawnEnemy(SelectEnemy());
    }

    private GameObject SelectEnemy()
    {
        var i = Random.Range(0, _enemiesLeft.Count);
        var chosenEnemy = _enemiesLeft[i];
        _enemiesLeft.RemoveAt(i);
        return chosenEnemy;
    }

    private void SpawnEnemy(GameObject enemy)
    {
        GameObject newEnemy = Instantiate(enemy, transform.position, transform.rotation);

        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScript.path = path;
        _gameOver.AddListener(() => enemyScript.SetState(EnemyState.Idle));

        newEnemy.GetComponent<HealthManager>().onDeath.AddListener(_ => EnemyDied(enemyScript));
    }

    private void EnemyDied(Enemy enemy)
    {
        if (++_enemiesKilled == _currentWave.Enemies.Count)
        {
            _allEnemiesDead.Invoke();
        }

        _enemyKilled.Invoke(enemy);
    }

    void OnDestroy()
    {
        _spawnTimer?.onTick.RemoveListener(SpawnNextEnemy);
    }
}