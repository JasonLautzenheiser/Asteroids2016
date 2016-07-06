using System;
using System.Collections.Generic;
using Asteroids.Entities.Player;
using Microsoft.Xna.Framework;

namespace Asteroids.Entities.Enemies
{
  public class Seeker : Enemy
  {
    private readonly Random rand = new Random();

    public Seeker(Vector2 position) : base()
    {
      Texture = Art.Asteroid;
      Radius = Texture.Width / 2.0f;
      PointValue = 2;
      DrawPriority = 1;
      Mass = 1f;
      Position = position;
      AddBehaviour(followPlayer(.05f));
    }

    private IEnumerable<int> followPlayer(float acceleration = 1f)
    {
      while (true)
      {
        Velocity += (Ship.Instance.Position - Position).ScaleTo(acceleration);
        Velocity = MathUtilities.ClampVelocity(Velocity);
        if (Velocity != Vector2.Zero)
          Rotation = Velocity.ToAngle();
        yield return 0;
      }
    }


  }
}