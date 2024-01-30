using System;
using System.Linq;
using Exceptions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class GameDirector : MonoBehaviour
{
    [Serializable]
    private enum GameState
    {
        Starting,
        Wave,
        Rest,
        GameOver
    }

    public struct SpawnerInfo
    {
        public UnityAction<Wave> onNewWave;
        public UnityEvent<Enemy> enemyKilled;
        public UnityEvent enemiesWiped;
        public UnityAction onGameOver;
    }

    [SerializeField] private static readonly float WaveInterval = 3f;
    [SerializeField] private static readonly float StartInterval = 5f;
    [SerializeField] private GameState _state;
    [SerializeField] private GameObject[] _enemies; //TODO: change for autogenerated
    [SerializeField] public int partialScore = 100;
    [SerializeField] public int totalScore = 0;
    private UnityEvent<Wave> _newWave = new();
    private UnityEvent _rest = new();
    private UnityEvent _gameOver = new();
    private Timer _waveTimer;
    private Timer _startTimer;
    private int _spawnCount = 0;
    private int _idleSpawns = 0;
    private int _waveCounter = -1;
    private int _enemiesPerWave = 10;
    private DateTime _startTime;
    [SerializeField] public int survivedTimeModifier = 10;
    [SerializeField] private float timeScoreInterval;
    [SerializeField] private int timeScoreMultiplier;
    private float _timeScoreTimer;
    private Stages _stages = new();

    public UnityEvent<int> OnPartialScoreChange;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text roundText;

    // Start is called before the first frame update
    void Start()
    {
        _state = GameState.Starting;

        _waveTimer = new Timer("WaveTimer", WaveInterval, true);
        _waveTimer.Stop();
        _waveTimer.onTick.AddListener(StartWave);

        _startTimer = new Timer("StartTimer", StartInterval, true);
        _startTimer.Restart();
        _startTimer.onTick.AddListener(StartGame);

        _startTime = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        _waveTimer.Update(Time.deltaTime);
        _startTimer?.Update(Time.deltaTime);
        _timeScoreTimer += Time.deltaTime;
        while (_timeScoreTimer > timeScoreInterval)
        {
            _timeScoreTimer -= timeScoreInterval;

            AddScore(timeScoreMultiplier);
        }

        scoreText.SetText($"SCORE: {partialScore}");
    }

    public void RegisterSpawn(SpawnerInfo spawnerInfo)
    {
        _newWave.AddListener(spawnerInfo.onNewWave);
        _gameOver.AddListener(spawnerInfo.onGameOver);
        spawnerInfo.enemiesWiped.AddListener(SubWaveCompleted);
        spawnerInfo.enemyKilled.AddListener(HandleEnemyKilled);
        if (_state == GameState.Wave)
        {
            _idleSpawns++;
        }

        _spawnCount++;
    }

    public void RegisterBase(HealthManager baseHealth)
    {
        baseHealth.onDeath.AddListener((_) => GameOver());
    }

    public void HandleEnemyKilled(Enemy enemy)
    {
        AddScore(enemy.GetScore());
    }

    private void StartWave()
    {
        _waveCounter++;
        _state = GameState.Wave;
        _idleSpawns = 0;
        var enemiesToSpawn = _enemies.Zip(_stages.GetProportions((float)_waveCounter / 5).ToArray(),
                (e, c) => (Enemy: e, Count: (int)Math.Round(c * _enemiesPerWave)))
            .SelectMany((t) => Enumerable.Repeat(t.Enemy, t.Count))
            .ToList();
        var wave = new Wave(enemiesToSpawn);
        _newWave.Invoke(wave);
        _waveTimer.Pause();
        roundText.SetText($"WAVE: {_waveCounter + 1}");
    }

    private void StartGame()
    {
        if (_state != GameState.Starting)
        {
            throw new InvalidStateException(String.Format("State should be Starting but is %s", _state));
        }

        _startTimer = null;
        StartWave();
    }

    private void SubWaveCompleted()
    {
        if (++_idleSpawns == _spawnCount)
        {
            StartRest();
        }
    }

    private void StartRest()
    {
        _state = GameState.Rest;
        _rest.Invoke();
        _waveTimer.Resume();
    }

    private void GameOver()
    {
        _state = GameState.GameOver;
        _gameOver.Invoke();
        // int survivedTime = DateTime.Now.Subtract(_startTime).Seconds;
        // totalScore += survivedTime * survivedTimeModifier;
    }

    public bool HasEnoughScore(int needed)
    {
        return partialScore >= needed;
    }

    public void Spend(int amount)
    {
        partialScore -= amount;
        OnPartialScoreChange.Invoke(partialScore);
    }

    private void AddScore(int score)
    {
        partialScore += score;
        totalScore += score;
        OnPartialScoreChange.Invoke(partialScore);
    }
}