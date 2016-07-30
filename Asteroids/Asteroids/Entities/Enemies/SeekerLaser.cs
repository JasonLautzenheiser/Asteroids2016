using System;
using Microsoft.Xna.Framework;

namespace Asteroids.Entities.Enemies
{
  public class SeekerLaser : Entity
  {
    private const int SPEED = 12;

    public SeekerLaser(Vector2 position, Vector2 velocity)
    {
      Texture = Art.SeekerShot;
      Position = position;
      Velocity = Vector2.Normalize(velocity) * SPEED;
      Rotation = Velocity.ToAngle();
      Damage = 25;
      DrawPriority = 1;
    }

    public override void Update()
    {
      if (Velocity.LengthSquared() > 0)
        Rotation = Velocity.ToAngle();

      Position += Velocity;

      if (!GameCore.Viewport.Bounds.Contains(Position.ToPoint()))
        ReadyToRemove = true;

    }

    public override void Die()
    {
      Random rand = new Random();
      for (int i = 0; i < 130; i++)
      {
        GameCore.ParticleManager.CreateParticle(Art.Glow, Position, Color.LightBlue, 10, 0.5f, new ParticleState() {Velocity = rand.NextVector2(0, 2), Type = ParticleType.None, LengthMultiplier = 0.75f});
      }

      base.Die();
    }
  }
}