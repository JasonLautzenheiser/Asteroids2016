using System;
using Asteroids.Entities.Player;
using Microsoft.Xna.Framework;

namespace Asteroids.Powerups
{
  public class Health : PowerUp
  {
    public Health(Vector2 position)
    {
      Type = PowerUpTypes.Health;
      Position = position;
      var texture = Art.PowerUpHealth;
      if (texture != null)
      {
        Radius = (float)(texture.Width / 2.0);
        Texture = texture;
      }
      DrawPriority = 1;
      CreationTime = DateTime.Now;
      PointValue = 5;
      ActiveDuration = 0;
    }

    public override void WasCaptured()
    {
      PlayerStatus.AddHealth(300);
      base.WasCaptured();
    }

    protected override void ExpirationCallback()
    {
    }

    public override bool IsActive()
    {
      return false;
    }

    public static PowerUp Create(Vector2 position)
    {
      var powerUp = new Health(position);
      return powerUp;
    }
  }
}