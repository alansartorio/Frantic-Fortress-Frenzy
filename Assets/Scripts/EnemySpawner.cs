using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static readonly float SpawnInterval = 1f;
    public static readonly float WaveInterval = 2f;
    public readonly int enemiesPerWave = 10;
    [SerializeField] public GameObject[] enemies;
    public GameObject rawPath;
    private EnemyPath _path;
    private Timer _waveTimer;
    private Timer _spawnTimer;
    /**
     * TODO: maybe load several waves in advance to make it better performant
     */
    public Wave currentWave;


    void Start()
    {
        _waveTimer = new Timer("Wave",WaveInterval, false);
        _waveTimer.Restart();
        _waveTimer.onTick.AddListener(SpawnWave);

        _spawnTimer = new Timer("Spawn", SpawnInterval, true);
        _spawnTimer.onTick.AddListener(SpawnNextEnemy);
        
        _path = rawPath.GetComponent<EnemyPath>();
        _path.target.GetComponent<HealthManager>().onDeath.AddListener((_) =>
        {
            enabled = false;
        });
    }

    void Update()
    {
        _waveTimer.Update(Time.deltaTime);
        _spawnTimer.Update(Time.deltaTime);
    }

    private void SpawnWave()
    {
        _waveTimer.Stop();
        currentWave = new Wave(enemiesPerWave, enemies);
        _spawnTimer.Restart();
    }
    
    private void SpawnNextEnemy()
    {
        if (currentWave != null)
        {
            var nextEnemy = currentWave.GetNextEnemy();
            if (nextEnemy == null)
            {
                currentWave = null;
                _spawnTimer.Stop();
                _waveTimer.Restart();
                return;
            }
            SpawnEnemy(nextEnemy);
        }
    }

    private void SpawnEnemy(GameObject enemy)
    {
        Enemy newEnemy = Instantiate(enemy, transform.position, transform.rotation).GetComponent<Enemy>();
        newEnemy.path = _path;
    }

    void OnDestroy()
    {
        _waveTimer?.onTick.RemoveListener(SpawnWave);
        _spawnTimer?.onTick.RemoveListener(SpawnNextEnemy);
    }
}
