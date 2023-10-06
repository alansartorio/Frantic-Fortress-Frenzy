
using System;

public enum DamageType
{
    Burning,
}

public static class DamageUtils
{
    public static Health AsHealth(float amount, DamageType type)
    {
        return type switch
        {
            DamageType.Burning => new Health(amount, 0, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
