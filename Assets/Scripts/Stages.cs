using System;

public struct EnemyProportions
{
    public float First { get; private set; }
    public float Second { get; private set; }
    public float Third { get; private set; }

    public EnemyProportions(float first, float second, float third)
    {
        First = first;
        Second = second;
        Third = third;
    }

    public float[] ToArray()
    {
        return new[]
        {
            First, Second, Third
        };
    }
}

public class Stages
{
    public EnemyProportions GetProportions(float t)
    {
        var weights = GetWeights(t);

        var sum = weights.First + weights.Second + weights.Third;
        return new EnemyProportions(weights.First / sum, weights.Second / sum, weights.Third / sum);
    }

    public EnemyProportions GetWeights(float t)
    {
        var stage = (float)Math.Truncate(t);
        var fract = t - stage;

        return stage switch
        {
            0 => new EnemyProportions(1, fract, 0),
            1 => new EnemyProportions(1, 1, fract),
            2 => new EnemyProportions(1 - (fract / 2f), 1, 1),
            3 => new EnemyProportions(0.5f, 1 - (fract / 2f), 1),
            _ => new EnemyProportions(0.5f, 0.5f, 1),
        };
    }
}