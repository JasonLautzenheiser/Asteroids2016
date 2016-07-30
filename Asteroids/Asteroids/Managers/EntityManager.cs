using System.Collections.Generic;
using System.Linq;
using Asteroids.Entities;
using Asteroids.Entities.Enemies;
using Asteroids.Entities.Player;
using Asteroids.Levels;
using Asteroids.Powerups;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Managers
{
  public static class EntityManager
  {
    static List<Entity> entities = new List<Entity>();
    static readonly List<Entity>  addedEntities = new List<Entity>();
    public static List<Enemy>  Enemies { get; private set; } = new List<Enemy>();
    static List<Laser> lasers = new List<Laser>();

    public static List<PowerUp> PowerUps { get; private set; }
    public static int PowerUpCount => PowerUps.Count;
    private static bool isUpdating;

    static EntityManager()
    {
      PowerUps = new List<PowerUp>();
    }

    public static int Count => entities.Count;

    public static void Add(Entity entity)
    {
      if (!isUpdating)
        addEntity(entity);
      else
        addedEntities.Add(entity);
    }

    private static void addEntity(Entity entity)
    {
      // does this level allow the enemy that is set to get created.
      if (entity is Enemy)
      {
        var etype = LevelManager.CurrentLevel.EnemiesAllowed.Any(p => p.EnemyType == entity.GetType());
        if (!etype) return;
      }

      entities.Add(entity);

      // add to special collections
      if (entity is Laser)
        lasers.Add(entity as Laser);
      else if (entity is Enemy)
        Enemies.Add(entity as Enemy);
      else if (entity is PowerUp)
        PowerUps.Add(entity as PowerUp);
    }

    public static void Update()
    {
      isUpdating = true;
      handleCollisions();

      foreach (var entity in entities)
        entity.Update();

      isUpdating = false;

      foreach (var addedEntity in addedEntities)
        addEntity(addedEntity);

      addedEntities.Clear();

      entities = entities.Where(x => !x.ReadyToRemove).ToList();
      lasers = lasers.Where(x => !x.ReadyToRemove).ToList();
      Enemies = Enemies.Where(x => !x.ReadyToRemove).ToList();
      PowerUps = PowerUps.Where(x => !x.ReadyToRemove).ToList();
    }

    public static void Draw(SpriteBatch batch)
    {
      foreach (var entity in entities.OrderByDescending(p=>p.DrawPriority))
        entity.Draw(batch);
    }

    static void handleCollisions()
    {
      foreach (var t in Enemies)
      {
      }

      foreach (var enemy in Enemies)
      {
        Enemy enemy1 = enemy;
        foreach (var laser in lasers.Where(laser => isColliding(enemy1, laser)))
        {
          enemy.WasShot();
          laser.ReadyToRemove = true;
        }

        if (enemy.IsActive && isColliding(Ship.Instance, enemy))
        {
          if (!Ship.Instance.AreShieldsUp)
          {
            enemy.WasShot();
            Ship.Instance.CurrentHealth -= enemy.Damage;
            if (Ship.Instance.CurrentHealth <= 0)
              killPlayer();
            break;
          }
          enemy.HandleCollision(Ship.Instance);
        }

        Enemy t1 = enemy;
        foreach (var s in Enemies.Where(s => t1 != s).Where(s => isColliding(t1, s)))
        {
          enemy.HandleCollision(s);
        }

      }

      foreach (var laser in entities.OfType<SeekerLaser>()  )
      {
        if (isColliding(Ship.Instance, laser))
        {
          if (!Ship.Instance.AreShieldsUp)
          {
            laser.Die();
            Ship.Instance.CurrentHealth -= laser.Damage;
            if (Ship.Instance.CurrentHealth <= 0)
              killPlayer();
            break;
          }
          else
          {
            laser.Die();
          }
        }
      }


      foreach (var powerUp in PowerUps)
      {
        if (isColliding(Ship.Instance, powerUp))
        {
          if (powerUp.IsActive())
          {
            Ship.Instance.AddPowerup(powerUp);
            powerUp.Active = true;
          }
          powerUp.WasCaptured();
        }
      }
    }

    private static void killPlayer()
    {
      if (!PlayerStatus.GodMode)
      {
        Enemies.ForEach(x => x.PlayerDeath());
        Ship.Instance.Kill();
        PowerUps.ForEach(x=>x.Die());
        entities.OfType<SeekerLaser>().ToList().ForEach(p => p.Die());
        EnemySpawner.Reset();
      }
    }

    private static bool isColliding(Entity a, Entity b)
    {
      if (a.ReadyToRemove || b.ReadyToRemove) return false;
      float aRadius = a.Radius;

      if (a is Ship)
        if (Ship.Instance.AreShieldsUp)
          aRadius += 20;
         
      float radius = aRadius + b.Radius;
      return Vector2.DistanceSquared(a.Position, b.Position) < radius*radius;
    }

    public static void KillAllEnemies()
    {
      EnemySpawner.Pause();
      Enemies.ForEach(x => x.WasShot(true));

      EnemySpawner.Reset();

    }
  }
}