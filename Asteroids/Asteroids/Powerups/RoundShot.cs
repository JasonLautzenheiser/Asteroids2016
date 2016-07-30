using System;
using Microsoft.Xna.Framework;

namespace Asteroids.Powerups
{
  public class RoundShot : PowerUp
  {
    public RoundShot(Vector2 position) : base()
    {
      Type = PowerUpTypes.RoundShot;
      Position = position;
      var texture = Art.PowerUpRoundShot;
      if (texture != null)
      {
        Radius = (float) (texture.Width/2.0);
        Texture = texture;
      }
      DrawPriority = 1;
      CreationTime = DateTime.Now;
      PointValue = 5;
      ActiveDuration = 8;
    }

    public override void WasCaptured()
    {
      base.WasCaptured();
      CapturedPowerUpParticles(Color.Lavender);
    }

    protected override void ExpirationCallback()
    {
    }

    public static PowerUp Create(Vector2 position)
    {
      var powerUp = new RoundShot(position);
      return powerUp;
    }

    public override bool IsActive()
    {
      return true;
    }
  }
}