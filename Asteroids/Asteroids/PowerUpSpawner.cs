using System;
using Microsoft.Xna.Framework;

namespace Asteroids
{
  public  enum PowerUpTypes
  {
    ExtraLife,
    MultiShoot
  }

  public static class PowerUpSpawner
  {
    private static readonly Random rand = new Random();
    private const int EXTRA_LIFE_CHANCE = 10;
    private const int MULTI_SHOOT_CHANCE = 3;

    public static void Update(Vector2 position)
    {
//      if (Ship.Instance.IsDead || EntityManager.Count >= 20) return;
      if (EntityManager.PowerUpCount < 1 && rand.Next(EXTRA_LIFE_CHANCE) == 0)
        EntityManager.Add(PowerUp.CreateExtraLife(position));

      if (EntityManager.PowerUpCount < 1 && rand.Next(MULTI_SHOOT_CHANCE) == 0)
        EntityManager.Add(PowerUp.CreateMultiShoot(position));

    }
  }
}