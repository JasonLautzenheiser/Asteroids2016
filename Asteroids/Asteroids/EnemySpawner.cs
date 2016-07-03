using System;
using Microsoft.Xna.Framework;

namespace Asteroids
{
  public static class EnemySpawner
  {
    private static readonly Random rand = new Random();
    private static float inverseSpawnChance = 90;
    private static float maxEntityCount = 20;
    private static float seekerChanceMultiplier = 6;

    public static void Update()
    {
      if (Ship.Instance.IsDead || EntityManager.Count >= maxEntityCount) return;
      if (rand.Next((int)inverseSpawnChance) == 0)
        EntityManager.Add(Enemy.CreateWanderer(getSpawnPosition()));

      if (rand.Next((int) inverseSpawnChance * (int)seekerChanceMultiplier) == 0)
        EntityManager.Add(Enemy.CreateSeeker(getSpawnPosition()));

      if (seekerChanceMultiplier > 3)
        seekerChanceMultiplier -= 0.00001f;
      
      if (maxEntityCount < 10)
        maxEntityCount += 0.001f;

      if (inverseSpawnChance > 30)
        inverseSpawnChance -= 0.000005f;
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
      inverseSpawnChance = 60;
      maxEntityCount = 20;
      seekerChanceMultiplier = 6;
    }
  }
}