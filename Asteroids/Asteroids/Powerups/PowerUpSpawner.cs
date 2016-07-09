using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Asteroids.Entities;
using Asteroids.Entities.Player;
using Asteroids.Managers;
using Microsoft.Xna.Framework;

namespace Asteroids.Powerups
{
  public  enum PowerUpTypes
  {
    ExtraLife,
    MultiShoot,
    Nuke,
    Shield
  }

  public enum PowerUpScarcity
  {
    Common,
    Rare,
    SuperRare
  }


  public static class PowerUpSpawner
  {
    private static readonly Random rand = new Random();
    private static readonly List<powerUp> powerUps;

    private static int countOfCommon;
    private static int countOfRare;
    private static int countOfSuperRare;

    private class powerUp
    {
      public PowerUpTypes Type { get; set; }
      public PowerUpScarcity Scarcity { get; set; }
    }

    static PowerUpSpawner()
    {
      powerUps = new List<powerUp>()
      {
        new powerUp() {Type = PowerUpTypes.ExtraLife, Scarcity = PowerUpScarcity.Rare},
        new powerUp() {Type = PowerUpTypes.Shield, Scarcity = PowerUpScarcity.Common},
        new powerUp() {Type = PowerUpTypes.MultiShoot, Scarcity = PowerUpScarcity.Common},
        new powerUp() {Type = PowerUpTypes.Nuke, Scarcity = PowerUpScarcity.SuperRare},
      }; 

      // cache powerup type counts
      countOfCommon = powerUps.Count(p => p.Scarcity == PowerUpScarcity.Common);
      countOfRare = powerUps.Count(p => p.Scarcity == PowerUpScarcity.Rare);
      countOfSuperRare = powerUps.Count(p => p.Scarcity == PowerUpScarcity.SuperRare);
    }

    public static void Update(Vector2 position)
    {
      if (EntityManager.PowerUpCount >= 1) return;

      var shouldWeSpawn = rand.Next(100) <= 50;

      Debug.WriteLine($"spawn: {shouldWeSpawn}");
      
      if (!shouldWeSpawn) return;

      var scarcityGenerator = rand.Next(100);
      Debug.WriteLine($"scarcity: {scarcityGenerator}");

      PowerUpScarcity type;
      if (scarcityGenerator < 5)
      {
        type = PowerUpScarcity.SuperRare;
        Debug.WriteLine($"Spawning Superrare");
      }
      else if (scarcityGenerator >= 5 && scarcityGenerator <= 25)
      {
        type = PowerUpScarcity.Rare;
        Debug.WriteLine($"Spawning rare");
      }
      else
      {
        Debug.WriteLine($"Spawning Common");
        type = PowerUpScarcity.Common;
      }


      switch (type)
      {
        case PowerUpScarcity.Common:
          spawnCommon(position);
          break;
        case PowerUpScarcity.Rare:
          spawnRare(position);
          break;
        case PowerUpScarcity.SuperRare:
          spawnSuperRare(position);
          break;
        default:
          return;
      }
    }

    private static void spawnCommon(Vector2 position)
    {
      if (rand.Next(2) == 0)
        if (PlayerStatus.ShieldsLeft < PlayerStatus.ShieldsPerLife)
        {
          EntityManager.Add(Shield.Create(position));
          Debug.Write($"Spawn Shield");
        }
        else
        {
          EntityManager.Add(ThreeWayShooter.Create(position));
          Debug.Write($"Spawn Mulishoot");

        }
    }

    private static void spawnRare(Vector2 position)
    {
      // if there become more than one rare powerup, then need to pick between them
      if (PlayerStatus.Lives != PlayerStatus.MaxLives)
      {
        EntityManager.Add(ExtraLife.Create(position));
        Debug.Write($"Spawn extra life");
      }
    }

    private static void spawnSuperRare(Vector2 position)
    {
      // if there become more than one superrare powerup, then need to pick between them
      EntityManager.Add(Nuke.Create(position));
      Debug.Write($"Nuke");

    }


    public static void Create(Vector2 position, PowerUpTypes type)
    {
      if (type==PowerUpTypes.ExtraLife)
        EntityManager.Add(ExtraLife.Create(position));

      if (type==PowerUpTypes.MultiShoot)
        EntityManager.Add(ThreeWayShooter.Create(position));

      if (type==PowerUpTypes.Shield)
        EntityManager.Add(Shield.Create(position));

      if (type==PowerUpTypes.Nuke)
        EntityManager.Add(Nuke.Create(position));

    }
  }
}