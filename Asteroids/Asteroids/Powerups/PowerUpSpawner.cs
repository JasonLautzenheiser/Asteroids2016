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
    Shield,
    RoundShot,
    Health
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
        new powerUp() {Type = PowerUpTypes.RoundShot, Scarcity = PowerUpScarcity.Common},
        new powerUp() {Type = PowerUpTypes.Health, Scarcity = PowerUpScarcity.Common},
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

      if (!shouldWeSpawn) return;

      var scarcityGenerator = rand.Next(100);

      PowerUpScarcity type;
      if (scarcityGenerator < 5)
      {
        type = PowerUpScarcity.SuperRare;
      }
      else if (scarcityGenerator >= 5 && scarcityGenerator <= 25)
      {
        type = PowerUpScarcity.Rare;
      }
      else
      {
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
      switch (rand.Next(4))
      {
        case 0:
          if (PlayerStatus.ShieldsLeft < PlayerStatus.ShieldsPerLife)
          {
            EntityManager.Add(Shield.Create(position));
          }
          break;
        case 1:
          EntityManager.Add(ThreeWayShooter.Create(position));
          break;
        case 2:
          EntityManager.Add(RoundShot.Create(position));
          break;
        case 3:
          if (Ship.Instance.CurrentHealth < Ship.Instance.MaxHealth * 0.8f)
          {
            EntityManager.Add(Health.Create(position));
          }
          break;
      }
    }

    private static void spawnRare(Vector2 position)
    {
      // if there become more than one rare powerup, then need to pick between them
      if (PlayerStatus.Lives != PlayerStatus.MaxLives)
      {
        EntityManager.Add(ExtraLife.Create(position));
      }
    }

    private static void spawnSuperRare(Vector2 position)
    {
      // if there become more than one superrare powerup, then need to pick between them
      EntityManager.Add(Nuke.Create(position));
    }


    public static void Create(Vector2 position, PowerUpTypes type)
    {
      if (type==PowerUpTypes.ExtraLife)
        EntityManager.Add(ExtraLife.Create(position));

      if (type==PowerUpTypes.MultiShoot)
        EntityManager.Add(ThreeWayShooter.Create(position));

      if (type==PowerUpTypes.RoundShot)
        EntityManager.Add(RoundShot.Create(position));

      if (type==PowerUpTypes.Shield)
        EntityManager.Add(Shield.Create(position));

      if (type==PowerUpTypes.Nuke)
        EntityManager.Add(Nuke.Create(position));

    }
  }
}