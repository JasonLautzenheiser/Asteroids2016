using System;
using System.Collections.Generic;
using Asteroids.Managers;
using Asteroids.Powerups;
using Asteroids.TextEntities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Entities.Enemies
{
  public class Wanderer : Enemy
  {
    private readonly Random rand = new Random();

    public Wanderer(Vector2 position) : base()
    {
      Texture = Art.Asteroid;
      Radius = Texture.Width / 2.0f;
      PointValue = 1;
      DrawPriority = 1;
      Mass = 1f;
      Position = position;
      Damage = 50;
      AddBehaviour(moveRandomly());
    }

    public override void WasShot(bool playerDeath = false, bool includePoints = true)
    {
      ReadyToRemove = true;

      //play explosion sound here.
      var soundExplosion = SoundEffects.ExplodeAsteroid.CreateInstance();
      soundExplosion.Volume = 0.1f;
      soundExplosion.Play();

      // give a little something for the kill
      float hue1 = rand.NextFloat(0, 6);
      float hue2 = (hue1 + rand.NextFloat(0, 2)) % 6f;
      Color color1 = ColorUtil.HSVToColor(hue1, 0.5f, 1);
      Color color2 = ColorUtil.HSVToColor(hue2, 0.5f, 1);

      for (int i = 0; i < 120; i++)
      {
        float speed = 2f * (1f - 1 / rand.NextFloat(1f, 10f));
        var state = new ParticleState
        {
          Velocity = rand.NextVector2(speed, speed),
          Type = ParticleType.EnemyExplosion,
          LengthMultiplier = 1f
        };
        Color particleColor = Color.Lerp(color1, color2, rand.NextFloat(0, 1));
        GameCore.ParticleManager.CreateParticle(Art.LineParticle, Position, particleColor, 50, 0.5f, state);
      }

      if(includePoints)
        PlayerStatus.AddPoints(PointValue);

      if (!playerDeath)
      {
        // create two smaller wanderers
        EntityManager.Add(new MiniWanderer(Position + new Vector2(rand.NextFloat(10,50), rand.NextFloat(10,50))));
        EntityManager.Add(new MiniWanderer(Position + new Vector2(rand.NextFloat(10, 50), rand.NextFloat(10, 50))));

        GameCore.TextManager.Add(new ActionScoreText(Position, PointValue.ToString()));
        PowerUpSpawner.Update(Position);
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
          Velocity = MathUtilities.ClampVelocity(Velocity,2.0f);
          Rotation -= 0.05f;

          var bounds = GameCore.Viewport.Bounds;
          bounds.Inflate(-Texture.Width, -Texture.Height);

          // if the enemy is outside the bounds, make it move away from the edge
          if (!bounds.Contains(Position.ToPoint()))
            direction = (GameCore.ScreenSize / 2 - Position).ToAngle() + rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2);

          yield return 0;
        }
      }
    }

  }
}