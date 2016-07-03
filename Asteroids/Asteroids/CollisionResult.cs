using Microsoft.Xna.Framework;

namespace Asteroids
{
  public struct CollisionResult
  {
    public bool HasCollided { get; set; }
    public Vector2 CollisionPoint { get; set; }
  }
}