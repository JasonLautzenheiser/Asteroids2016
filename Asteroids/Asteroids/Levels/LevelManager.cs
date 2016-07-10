using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Entities;
using Asteroids.Entities.Enemies;
using Asteroids.Managers;
using Asteroids.TextEntities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Levels
{
  public static class LevelManager
  {
    private static readonly List<Level> levels;
    private static float levelPause;
    public static Level CurrentLevel { get; private set; }
    private static bool levelIntermission = false;

    static LevelManager()
    {
      levels = new List<Level>();
    }

    public static void InitLevels(int startLevel = 1)
    {
      levels.Add(new Level
      {
        Number = 1,
        EnemiesAllowed = new List<LevelEnemy>
        {
          new LevelEnemy { EnemyType = typeof(Wanderer), MaxNumber = 10, SpawnRate = 1.0f, AutoSpawn = true},
        }
      });

      levels.Add(new Level()
      {
        Number = 2,
        EnemiesAllowed = new List<LevelEnemy>
        {
          new LevelEnemy { EnemyType = typeof(Wanderer), MaxNumber = 10, SpawnRate = 1.0f, AutoSpawn = true},
          new LevelEnemy { EnemyType = typeof(MiniWanderer), MaxNumber = 10, AutoSpawn = false},
        }
      });

      CurrentLevel = levels.FirstOrDefault(p => p.Number == startLevel);
    }

    public static Level LoadLevel(int number)
    {
      var levelFound = levels.FirstOrDefault(p => p.Number == number);
      CurrentLevel = levelFound;
      return levelFound;
    }

    public static void Update()
    {

      if (levelIntermission)
      {
        levelPause -= (float)GameCore.GameTime.ElapsedGameTime.TotalSeconds;
        if (levelPause < 0.0f)
        {
          int number = CurrentLevel.Number;
          GameCore.TextManager.Remove("LevelComplete");
          LoadLevel(number+1);
          levelIntermission = false;
        }
      }
      else
      {
        if (isLevelComplete())
        {
          GameCore.TextManager.Add(new GenericText("LevelComplete", new Vector2(GameCore.Viewport.Width/2 - 65, 100), "Level Complete", Color.White));
          levelIntermission = true;
          levelPause = 2.0f;
        }
      }
    }

    private static bool isLevelComplete()
    {
      var complete = CurrentLevel.EnemiesAllowed.Count(p => p.AutoSpawn && p.MaxNumber == p.Created) == CurrentLevel.EnemiesAllowed.Count (p=>p.AutoSpawn);

      if (complete && EntityManager.Enemies.Count == 0) return true;

      return false;
    }
  }
}