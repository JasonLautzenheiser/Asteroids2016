using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Asteroids.Powerups
{
  public class Nuke : PowerUp
  {
    private Nuke(Vector2 position) : base()
    {
      Type = PowerUpTypes.Nuke;
      Position = position;
      var texture = Art.PowerUpNuke;
      if (texture != null)
      {
        Radius = (float)(texture.Width / 2.0);
        Texture = texture;
      }
      DrawPriority = 1;
      CreationTime = DateTime.Now;
      PointValue = 15;
      ActiveDuration = 0;
    }

    protected override void ExpirationCallback()
    {
      Debug.WriteLine("Nuke expired:  Who cares");
    }

    public override bool IsActive()
    {
      return false;
    }

    public static PowerUp Create(Vector2 position)
    {
      var powerUp = new Nuke(position);
      return powerUp;
    }
  }
}