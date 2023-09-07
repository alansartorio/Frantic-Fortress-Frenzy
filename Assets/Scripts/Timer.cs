using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Timer
{
    public readonly UnityEvent onTick = new();
    private readonly float _period;
    private readonly bool _tickOnStart;
    private bool _playing = false;
    private float _time = 0;
    private bool _hold = false;
    private readonly string _name = "Default";

    public Timer(float period, bool tickOnStart)
    {
        this._period = period;
        this._tickOnStart = tickOnStart;
        Reset();
    }
    
    public Timer(string name, float period, bool tickOnStart) : this(period, tickOnStart)
    {
        _name = name;
    }

    public void Resume()
    {
        _playing = true;
        _hold = false;
    }

    public void Hold() {
        _hold = true;
    }

    public void Pause()
    {
        _playing = false;
        _hold = false;
    }

    public void Stop()
    {
        Pause();
        Reset();
    }

    public void Update(float deltaTime)
    {
        if (_playing)
        {
            _time += deltaTime;
            if (_hold && _time >= _period) {
                _hold = false;
                Pause();
                return;
            }
            while (_time >= _period)
            {
                _time -= _period;
                onTick.Invoke();
            }
        }
    }

    public void Reset()
    {
        _time = _tickOnStart ? _period : 0;
    }

    public void Restart()
    {
        Reset();
        Resume();
    }

    public override string ToString()
    {
        return $"Timer '{_name}': (time: {_time:.3f} | period: {_period:.3f})";
    }
}