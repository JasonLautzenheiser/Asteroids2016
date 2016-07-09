using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Asteroids.Powerups
{
  public class ThreeWayShooter : PowerUp
  {
    private ThreeWayShooter(Vector2 position) : base()
    {
      Type = PowerUpTypes.MultiShoot;
      Position = position;
      var texture = Art.PowerUpThreeWay;
      if (texture != null)
      {
        Radius = (float)(texture.Width / 2.0);
        Texture = texture;
      }
      DrawPriority = 1;
      CreationTime = DateTime.Now;
      PointValue = 5;
      ActiveDuration = 15;
    }

    public override void WasCaptured()
    {
      base.WasCaptured();
      CapturedPowerUpParticles(Color.Red);
    }

    protected override void ExpirationCallback()
    {
      Debug.WriteLine("Threeway shooter expires callback");
    }

    public static PowerUp Create(Vector2 position)
    {
      var powerUp = new ThreeWayShooter(position);
      return powerUp;
    }

    public override bool IsActive()
    {
      return true;
    }
  }
}