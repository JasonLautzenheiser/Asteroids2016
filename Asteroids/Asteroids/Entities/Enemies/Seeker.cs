using System;
using System.Collections.Generic;
using System.Diagnostics;
using Asteroids.Entities.Player;
using Asteroids.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Entities.Enemies
{
  public class Seeker : Enemy
  {
    private readonly Random rand = new Random();

    public Seeker(Vector2 position) : base()
    {
      Texture = Art.Seeker;
      Radius = Texture.Width / 2.0f;
      PointValue = 10;
      DrawPriority = 1;
      Mass = 1f;
      Damage = 75;
      Position = position;
      AddBehaviour(followPlayer(.05f));
      AddBehaviour(fireMissile());
    }

    private IEnumerable<int> fireMissile()
    {
      while (true)
      {
        if (rand.Next(0,50)==7)
        {
          var trajectory = (Ship.Instance.Position - Position).ScaleTo(1.0f);
          var shot = new SeekerLaser(Position, trajectory);
          EntityManager.Add(shot);
        }
        yield return 0;
      }

    }

    private IEnumerable<int> followPlayer(float acceleration = 1f)
    {
      while (true)
      {
          Velocity += (Ship.Instance.Position - Position).ScaleTo(acceleration);
          Velocity = MathUtilities.ClampVelocity(Velocity, 7f);
        yield return 0;
      }
    }

    public override void Update()
    {
      base.Update();
    }

    public override void Draw(SpriteBatch batch)
    {

      base.Draw(batch);
    }
  }
}