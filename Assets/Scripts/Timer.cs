using UnityEngine;
using UnityEngine.Events;

public class Timer
{
    public UnityEvent onTick = new UnityEvent();
    private float period;
    private float time = 0;

    public Timer(float period)
    {
        this.period = period;
    }

    public void Update(float deltaTime)
    {
        time += deltaTime;
        while (time >= period)
        {
            time -= period;
            onTick.Invoke();
        }
    }
}