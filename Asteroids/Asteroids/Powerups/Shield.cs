using System;
using System.Diagnostics;
using Asteroids.Entities.Player;
using Microsoft.Xna.Framework;

namespace Asteroids.Powerups
{
  public class Shield : PowerUp
  {
    private Shield(Vector2 position) : base()
    {
      Type = PowerUpTypes.Shield;
      Position = position;
      var texture = Art.PowerUpShield;
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
      PlayerStatus.AddShield();
      Ship.Instance.NewShieldParticles();
      base.WasCaptured();
      CapturedPowerUpParticles(Color.Green);
    }


    protected override void ExpirationCallback()
    {
      Debug.WriteLine("Shield expired....who cares");
    }

    public override bool IsActive()
    {
      return false;
    }

    public static PowerUp Create(Vector2 position)
    {
      var powerUp = new Shield(position);
      return powerUp;
    }
  }
}