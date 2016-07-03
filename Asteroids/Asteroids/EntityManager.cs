using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
  public static class EntityManager
  {
    static List<Entity> entities = new List<Entity>();
    static readonly List<Entity>  addedEntities = new List<Entity>();
    static List<Enemy>  enemies = new List<Enemy>();
    static List<Laser> lasers = new List<Laser>();

    public static List<PowerUp> PowerUps { get; private set; }
    public static int PowerUpCount { get { return PowerUps.Count; } }

    private static bool isUpdating;

    static EntityManager()
    {
      PowerUps = new List<PowerUp>();
    }

    public static int Count { get { return entities.Count; } }

    public static void Add(Entity entity)
    {
      if (!isUpdating)
        addEntity(entity);
      else
        addedEntities.Add(entity);
    }

    private static void addEntity(Entity entity)
    {
      entities.Add(entity);
      if (entity is Laser)
        lasers.Add(entity as Laser);
      else if (entity is Enemy)
        enemies.Add(entity as Enemy);
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
      enemies = enemies.Where(x => !x.ReadyToRemove).ToList();
      PowerUps = PowerUps.Where(x => !x.ReadyToRemove).ToList();
    }

    public static void Draw(SpriteBatch batch)
    {
      batch.DrawString(Font.Arial20,string.Format("Entities:{0}", entities.Count),new Vector2(20,70),Color.Green );
      foreach (var entity in entities.OrderByDescending(p=>p.DrawPriority))
        entity.Draw(batch);
    }

    static void handleCollisions()
    {
      foreach (var t in enemies)
      {
        Enemy t1 = t;
        foreach (var s in enemies.Where(s => t1 != s).Where(s => isColliding(t1, s)))
        {
          t.HandleCollision(s);
        }
      }

      foreach (var enemy in enemies)
      {
        Enemy enemy1 = enemy;
        foreach (var laser in lasers.Where(laser => isColliding(enemy1, laser)))
        {
          enemy.WasShot();
          laser.ReadyToRemove = true;
        }
      }

      foreach (var enemy in enemies)
      {
        if (enemy.IsActive && isColliding(Ship.Instance, enemy))
        {
          if (!Ship.Instance.AreShieldsUp)
          {
            killPlayer();
            break;
          }
          enemy.HandleCollision(Ship.Instance);
        }
      }

      foreach (var powerUp in PowerUps)
      {
        if (isColliding(Ship.Instance, powerUp))
        {
          Ship.Instance.AddPowerup(powerUp);
          powerUp.Active = true;
          powerUp.WasCaptured();
        }
      }
    }

    private static void killPlayer()
    {
      Ship.Instance.Kill();
      enemies.ForEach(x=>x.PlayerDeath());
      PowerUps.ForEach(x=>x.ReadyToRemove=true);
      EnemySpawner.Reset();
    }

    private static bool isColliding(Entity a, Entity b)
    {
      float aRadius = a.Radius;

      if (a is Ship)
        if (Ship.Instance.AreShieldsUp)
          aRadius += 20;
         
      float radius = aRadius + b.Radius;
      return !a.ReadyToRemove && !b.ReadyToRemove && Vector2.DistanceSquared(a.Position, b.Position) < radius*radius;
    }
  }
}