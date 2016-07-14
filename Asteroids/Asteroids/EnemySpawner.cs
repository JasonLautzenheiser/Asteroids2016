using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Asteroids.Entities;
using Asteroids.Entities.Enemies;
using Asteroids.Entities.Player;
using Asteroids.Levels;
using Asteroids.Managers;
using Asteroids.TextEntities;
using Asteroids.Utilities;
using Microsoft.Xna.Framework;

namespace Asteroids
{
  public static class EnemySpawner
  {
    private static readonly Random rand = new Random();
    private static float inverseSpawnChance = 90;
    private static float maxEntityCount = 20;
    private static float seekerChanceMultiplier = 6;
    private static bool pause = false;
    private static float fStartDelay = 0.5f;

    private static double lastEnemySpawn;

    public static void StartLevel()
    {
      LevelManager.CurrentLevel.EnemiesAllowed.ForEach(p=> { p.Created = 0;
                                                             p.LastSpawn = 0.0;
      });
      
    }

    public static void Update()
    {
      // determine if we should spawn
      fStartDelay -= (float)GameCore.GameTime.ElapsedGameTime.TotalSeconds;
      if (fStartDelay > 0) return;
      if (Ship.Instance.IsDead || EntityManager.Count >= maxEntityCount || pause) return;

      // can any enemy spawn yet?
      if (rand.Next(2) == 0)
      {
        foreach (var enemy in LevelManager.CurrentLevel.EnemiesAllowed.Where(p=>p.AutoSpawn && p.MaxNumber > p.Created).OrderByDescending(p => p.SpawnRate))
        {
          Debug.WriteLine($"Trying to spawn: {enemy.EnemyType.FullName}");
          if (GameCore.GameTime.TotalGameTime.TotalSeconds - enemy.LastSpawn > enemy.SpawnRate)
          {
            Debug.WriteLine($"spawn: {enemy.EnemyType.FullName}");
            var enemyToSpawn = enemy;
            EntityManager.Add(enemyToSpawn.EnemyType.GetInstance<Entity>(getSpawnPosition()));
            enemy.LastSpawn = GameCore.GameTime.TotalGameTime.TotalSeconds;
            enemy.Created++;
            break;
          }
        }
      }
    }

    private static Vector2 getSpawnPosition()
    {
      Vector2 pos;
      do
      {
        pos = new Vector2(rand.Next((int) GameCore.ScreenSize.X), rand.Next((int) GameCore.ScreenSize.Y));
      } while (Vector2.DistanceSquared(pos, Ship.Instance.Position) < 250*250);
      return pos;
    }

    public static void Reset()
    {
      fStartDelay = 1.5f;
      inverseSpawnChance = 90;
      maxEntityCount = 20;
      seekerChanceMultiplier = 6;
      StartLevel();
      pause = false;

    }

    public static void Pause()
    {
      pause = true;
    }

    private class LevelEnemyExtra
    {
      public LevelEnemy Enemy { get; set; }
      public int Created { get; set; }
    }
  }
}