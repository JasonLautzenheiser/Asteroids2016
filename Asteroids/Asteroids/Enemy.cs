﻿using System;
using System.Collections.Generic;
using Asteroids.Powerups;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
  public class Enemy : Entity
  {
    public double NoCollisionDecay = 0.5;
    public double NoCollisionLife = 2;
    private readonly List<IEnumerator<int>> behaviours = new List<IEnumerator<int>>();
    public int PointValue { get; private set; }
    private readonly Random rand = new Random();
    private const int TIME_UNTIL_START = 0;
    public bool IsActive { get { return TIME_UNTIL_START <= 0; } }

    public const float MAX_VELOCITY = 5f;

    public Enemy(Texture2D texture, Vector2 position)
    {
      Position = position;
      if (texture != null)
      {
        Radius = texture.Width/2;
        Texture = texture;
      }
      color = Color.White;
      PointValue = 1;
      DrawPriority = 1;
      Mass = 1f;
    }


    private void addBehaviour(IEnumerable<int> behaviour)
    {
      behaviours.Add(behaviour.GetEnumerator());
    }

    private void applyBehaviours()
    {
      for (int i = 0; i < behaviours.Count; i++)
      {
        if (!behaviours[i].MoveNext())
          behaviours.RemoveAt(i--);
      }
    }

    public override Rectangle GetBoundingRectangle()
    {
      return new Rectangle((int) Position.X, (int) Position.Y, (int) Radius, (int) Radius);
    }

    public void PlayerDeath()
    {
      WasShot(true);
    }

    public void WasShot(bool playerDeath = false)
    {
      ReadyToRemove = true;

      //play explosion sound here.
      var soundExplosion = SoundEffects.ExplodeAsteroid.CreateInstance();
      soundExplosion.Volume = 0.1f;
      soundExplosion.Play();

      float hue1 = rand.NextFloat(0, 6);
      float hue2 = (hue1 + rand.NextFloat(0, 2))%6f;
      Color color1 = ColorUtil.HSVToColor(hue1, 0.5f, 1);
      Color color2 = ColorUtil.HSVToColor(hue2, 0.5f, 1);

      for (int i = 0; i < 120; i++)
      {
        float speed = 18f*(1f - 1/rand.NextFloat(1f, 10f));
        var state = new ParticleState
                      {
                        Velocity = rand.NextVector2(speed, speed),
                        Type = ParticleType.EnemyExplosion,
                        LengthMultiplier = 1f
                      };
        Color particleColor = Color.Lerp(color1, color2, rand.NextFloat(0, 1));
        GameCore.ParticleManager.CreateParticle(Art.LineParticle, Position, particleColor,190,1.5f, state);
      }

      if (!playerDeath)
      {
        PlayerStatus.AddPoints(PointValue);
        PowerUpSpawner.Update(Position);
      }
    }

    public override void Update()
    {
      applyBehaviours();

      if (GameCore.Instance.Window != null) this.WrapPositionIfCrossing(GameCore.Instance.Window.ClientBounds);

      Position += Velocity;

//      Velocity *= 0.8f;

      base.Update();

      if (NoCollisionLife > 0)
        NoCollisionLife -= NoCollisionDecay*GameCore.GameTime.ElapsedGameTime.TotalSeconds;
    }

    public static Enemy CreateWanderer(Vector2 position)
    {
      var enemy = new Enemy(Art.Asteroid, position);
      enemy.addBehaviour(enemy.moveRandomly());
      return enemy;
    }

    public static Entity CreateSeeker(Vector2 position)
    {
      var enemy = new Enemy(Art.Asteroid, position);
      enemy.addBehaviour(enemy.followPlayer(.05f));
      return enemy;
    }

    public void HandleCollision(Entity other)
    {
      Vector2 cofMass = (Velocity + other.Velocity)/2;
      
      Vector2 normal1 = Position - other.Position;
      normal1.Normalize();

      Velocity -= cofMass;
      Velocity = Vector2.Reflect(Velocity, normal1);
      Velocity += cofMass;

      Velocity += 100 * normal1 / (normal1.LengthSquared() + 1); 
      Velocity = clampVelocity(Velocity);
      

      if (other.GetType() != typeof(Ship))
      {
        Vector2 normal2 = other.Position - Position;
        normal2.Normalize();

        other.Velocity -= cofMass;
        other.Velocity = Vector2.Reflect(Velocity, normal2);
        other.Velocity += cofMass;

        other.Velocity += 100 * normal2 / (normal2.LengthSquared() + 1);
        other.Velocity = clampVelocity(other.Velocity);
      }
    }


    // behaviors
    private IEnumerable<int> followPlayer(float acceleration = 1f)
    {
      while (true)
      {
        Velocity += (Ship.Instance.Position - Position).ScaleTo(acceleration);
        Velocity = clampVelocity(Velocity);
        if (Velocity != Vector2.Zero)
          Rotation = Velocity.ToAngle();
        yield return 0;
      }
    }

    private IEnumerable<int> moveRandomly()
    {
      float direction = rand.NextFloat(0, MathHelper.TwoPi);

      while (true)
      {
        direction += rand.NextFloat(-0.1f, 0.1f);
        direction = MathHelper.WrapAngle(direction);

        for (int i = 0; i < 6; i++)
        {
          Velocity += MathUtilities.FromPolar(direction, 0.01f);
          Velocity = clampVelocity(Velocity);
          Rotation -= 0.05f;

          var bounds = GameCore.Viewport.Bounds;
          bounds.Inflate(-Texture.Width, -Texture.Height);

          // if the enemy is outside the bounds, make it move away from the edge
          if (!bounds.Contains(Position.ToPoint()))
            direction = (GameCore.ScreenSize/2 - Position).ToAngle() + rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2);

          yield return 0;
        }
      }
    }

    private static Vector2 clampVelocity(Vector2 velocity)
    {
      if (velocity.Length() > MAX_VELOCITY)
        return Vector2.Normalize(velocity) * MAX_VELOCITY;
      return velocity;
    }


  }
}