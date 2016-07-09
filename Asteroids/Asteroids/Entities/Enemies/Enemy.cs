using System;
using System.Collections.Generic;
using Asteroids.Entities.Player;
using Asteroids.Powerups;
using Asteroids.TextEntities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Entities.Enemies
{
  public abstract class Enemy : Entity
  {
    public double NoCollisionDecay = 0.5;
    public double NoCollisionLife = 2;
    private readonly List<IEnumerator<int>> behaviours = new List<IEnumerator<int>>();
    public int PointValue { get; set; }
    private readonly Random rand = new Random();
    private const int TIME_UNTIL_START = 0;
    public bool IsActive => TIME_UNTIL_START <= 0;

    protected Enemy()
    {
    }

    protected void AddBehaviour(IEnumerable<int> behaviour)
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
      WasShot(true,false);
    }



    public virtual void WasShot(bool playerDeath = false, bool keepPoints = true)
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
        float speed = 2f*(1f - 1/rand.NextFloat(1f, 10f));
        var state = new ParticleState
                      {
                        Velocity = rand.NextVector2(speed, speed),
                        Type = ParticleType.EnemyExplosion,
                        LengthMultiplier = 1f
                      };
        Color particleColor = Color.Lerp(color1, color2, rand.NextFloat(0, 1));
        GameCore.ParticleManager.CreateParticle(Art.LineParticle, Position, particleColor,25,0.5f, state);
      }

      if (keepPoints)
        PlayerStatus.AddPoints(PointValue);

      if (!playerDeath)
      {
        GameCore.TextManager.Add(new ActionScoreText(Position, PointValue.ToString()));
        PowerUpSpawner.Update(Position);
      }
    }

    public override void Update()
    {
      applyBehaviours();

      if (GameCore.Instance.Window != null) this.WrapPositionIfCrossing(GameCore.Instance.Window.ClientBounds);

      Position += Velocity ;

      base.Update();

      if (NoCollisionLife > 0)
        NoCollisionLife -= NoCollisionDecay*GameCore.GameTime.ElapsedGameTime.TotalSeconds;
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
      Velocity = MathUtilities.ClampVelocity(Velocity);
      

      if (other.GetType() != typeof(Ship))
      {
        Vector2 normal2 = other.Position - Position;
        normal2.Normalize();

        other.Velocity -= cofMass;
        other.Velocity = Vector2.Reflect(Velocity, normal2);
        other.Velocity += cofMass;

        other.Velocity += 100 * normal2 / (normal2.LengthSquared() + 1);
        other.Velocity = MathUtilities.ClampVelocity(other.Velocity);
      }
    }
    
  }
}