﻿using System;
using Asteroids.Entities;
using Asteroids.Entities.Enemies;
using Asteroids.Entities.Player;
using Asteroids.Managers;
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

    public static void Update()
    {
      fStartDelay -= (float)GameCore.GameTime.ElapsedGameTime.TotalSeconds;
      if (fStartDelay > 0) return;

      if (Ship.Instance.IsDead || EntityManager.Count >= maxEntityCount || pause) return;
      if (rand.Next((int)inverseSpawnChance) == 0)
        EntityManager.Add(new Wanderer(getSpawnPosition()));

//      if (rand.Next((int)inverseSpawnChance) == 0)
//        EntityManager.Add(new MiniWanderer(getSpawnPosition()));

      if (rand.Next((int) inverseSpawnChance * (int)seekerChanceMultiplier) == 0)
        EntityManager.Add(new Seeker(getSpawnPosition()));


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
      fStartDelay = 1.5f;
      inverseSpawnChance = 90;
      maxEntityCount = 20;
      seekerChanceMultiplier = 6;
      pause = false;
    }

    public static void Pause()
    {
      pause = true;
    }
  }
}