using System;
using Microsoft.Xna.Framework;

namespace Asteroids
{
  public static class MathUtilities
  {
    public static Vector2 FromPolar(float angle, float magnitude)
    {
      return magnitude * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
    }

    

    public static Vector2 ClampVelocity(Vector2 velocity, float maxVelocity = 5.0f)
    {
      if (velocity.Length() > maxVelocity)
        return Vector2.Normalize(velocity) * maxVelocity;
      return velocity;
    }

    public static float ClampRotation(float rotation, float maxDegrees = 180f)
    {
      if (rotation > maxDegrees)
        return maxDegrees;
      return rotation;
    }

  }
}