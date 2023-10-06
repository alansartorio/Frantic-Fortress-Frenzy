using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private static readonly float SpawnInterval = 1f;
    public EnemyPath path;
    private Timer _spawnTimer;
    /**
     * TODO: maybe load several waves in advance to make it better performant
     */
    [SerializeField] private Wave _currentWave;
    private int _enemiesSpawned = 0;
    private int _enemiesKilled = 0;
    private readonly UnityEvent _allEnemiesDead = new();
    private readonly UnityEvent _gameOver = new();


    void Start()
    {
        _spawnTimer = new Timer("Spawn", SpawnInterval, true);
        _spawnTimer.Stop();
        _spawnTimer.onTick.AddListener(SpawnNextEnemy);
        
        var director = FindObjectOfType<GameDirector>();
        director.RegisterSpawn(new GameDirector.SpawnerInfo()
        {
            enemiesWiped = _allEnemiesDead,
            onGameOver = () => _gameOver.Invoke(),
            onNewWave = SpawnWave
        });
    }

    void Update()
    {
        _spawnTimer.Update(Time.deltaTime);
    }

    private void SpawnWave(Wave wave)
    {
        _currentWave = wave;
        _enemiesSpawned = 0;
        _enemiesKilled = 0;
        _spawnTimer.Restart();
    }
    
    private void SpawnNextEnemy()
    {
        if (_enemiesSpawned == _currentWave.EnemyCount) return;
        SpawnEnemy(SelectEnemy());
    }

    private GameObject SelectEnemy()
    {
        return _currentWave.EnemyTypes[Random.Range(0,_currentWave.EnemyTypes.Length)];
    }

    private void SpawnEnemy(GameObject enemy)
    {
        GameObject newEnemy = Instantiate(enemy, transform.position, transform.rotation);
        
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScript.path = path;
        _gameOver.AddListener(() => enemyScript.SetState(EnemyState.Idle));
        
        newEnemy.GetComponent<HealthManager>().onDeath.AddListener(_ => EnemyDied());

        _enemiesSpawned++;
    }

    private void EnemyDied()
    {
        if (++_enemiesKilled == _currentWave.EnemyCount)
        {
            _allEnemiesDead.Invoke();
        }
    }

    void OnDestroy()
    {
        _spawnTimer?.onTick.RemoveListener(SpawnNextEnemy);
    }
}
