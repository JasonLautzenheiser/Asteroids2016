using System;
using Asteroids.Managers;
using Microsoft.Xna.Framework;

namespace Asteroids
{
  public enum ParticleType {None, ShipExhaust, EnemyExplosion,
    Shield
  }
  public struct ParticleState
  {
    public Vector2 Velocity;
    public ParticleType Type;
    public float LengthMultiplier;

    public ParticleState(Vector2 velocity, ParticleType type, float lengthMultiplier=1f)
    {
      Velocity = velocity;
      Type = type;
      LengthMultiplier = lengthMultiplier;
    }

    public static void UpdateParticle(ParticleManager<ParticleState>.Particle particle)
    {
      var vel = particle.State.Velocity;
      float speed = vel.Length();

      // vector2.Add is slightly faster that "x.Position += vel;" since these are passed
      // by reference and don't need to be copied.
      Vector2.Add(ref particle.Position, ref vel, out particle.Position);


      // fade the particle if percentlife or speed is low
      float alpha = Math.Min(1, Math.Min(particle.PercentLife * 2, speed * 1f));
      alpha *= alpha;

      particle.Tint.A = (byte)(255 * alpha);

      if (particle.State.Type == ParticleType.Shield)
        particle.Scale.X = particle.State.LengthMultiplier * Math.Min(Math.Min(1f, 0.1f * speed + 0.1f), alpha);
      else
        particle.Scale.X = particle.State.LengthMultiplier * Math.Min(Math.Min(1f, 0.2f * speed + 0.1f), alpha);

      particle.Rotation = vel.ToAngle();

      // performance enhancement
      if (Math.Abs(vel.X) + Math.Abs(vel.Y) < 0.00000001f)
        vel = Vector2.Zero;
      else if (particle.State.Type == ParticleType.ShipExhaust)
        vel *= 0.94f;
      else if (particle.State.Type == ParticleType.EnemyExplosion)
        vel *= 0.98f;
      else
        vel *= 0.97f + Math.Abs(particle.Position.X) % 0.04f;
      
      
      particle.State.Velocity = vel;
    }
  }
}