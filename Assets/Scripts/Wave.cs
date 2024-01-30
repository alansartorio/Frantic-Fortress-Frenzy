using System.Collections.Generic;
using UnityEngine;

public class Wave
{
    private readonly List<GameObject> _enemies;

    public Wave(List<GameObject> enemies)
    {
        _enemies = enemies;
    }

    public List<GameObject> Enemies => _enemies;
}
