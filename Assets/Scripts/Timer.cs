using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Timer
{
    public UnityEvent onTick = new();
    private readonly float period;
    private readonly bool tickOnStart;
    private bool playing = false;
    private float time = 0;
    private bool hold = false;
    private string _name = "Default";

    public Timer(float period, bool tickOnStart)
    {
        this.period = period;
        this.tickOnStart = tickOnStart;
        Reset();
    }
    
    public Timer(string name, float period, bool tickOnStart) : this(period, tickOnStart)
    {
        _name = name;
    }

    public void Resume()
    {
        playing = true;
        hold = false;
    }

    public void Hold() {
        hold = true;
    }

    public void Pause()
    {
        playing = false;
        hold = false;
    }

    public void Stop()
    {
        Pause();
        Reset();
    }

    public void Update(float deltaTime)
    {
        if (playing)
        {
            time += deltaTime;
            if (hold && time >= period) {
                hold = false;
                Pause();
                return;
            }
            while (time >= period)
            {
                time -= period;
                onTick.Invoke();
            }
        }
    }

    public void Reset()
    {
        time = tickOnStart ? period : 0;
    }

    public void Restart()
    {
        Reset();
        Resume();
    }
}