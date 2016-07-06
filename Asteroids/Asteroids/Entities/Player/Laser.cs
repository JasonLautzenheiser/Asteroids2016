using Microsoft.Xna.Framework;

namespace Asteroids.Entities.Player
{
  public class Laser : Entity
  {
    private const int SPEED = 10;
    
    public Laser(Vector2 position, Vector2 velocity)
    {
      Texture = Art.Bullet;
      Position = position;
      Velocity = Vector2.Normalize(velocity) * SPEED;
      Rotation = Velocity.ToAngle();
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
  }
}