using System;
using System.Collections.Generic;

namespace Asteroids.Levels
{
  public class Level
  {
    public int Number { get; set; }
    public string Name { get; set; }

    public List<LevelEnemy> EnemiesAllowed { get; set; }
  }


  public class LevelEnemy
  {
    public Type EnemyType { get; set; }
    public int MaxNumber { get; set; } = 20;
    public float SpawnRate { get; set; } = 1;
    public bool AutoSpawn { get; set; } = true;
    public int Created { get; set; }
  }
}