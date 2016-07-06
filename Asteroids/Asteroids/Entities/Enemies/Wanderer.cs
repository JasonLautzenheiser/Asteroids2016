using System;
using System.Collections.Generic;
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
      AddBehaviour(moveRandomly());
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
          Velocity = MathUtilities.ClampVelocity(Velocity,5.0f);
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