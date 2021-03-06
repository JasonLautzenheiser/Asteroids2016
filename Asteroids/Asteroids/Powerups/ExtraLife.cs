﻿using System;
using System.Diagnostics;
using Asteroids.Entities;
using Asteroids.Entities.Player;
using Microsoft.Xna.Framework;

namespace Asteroids.Powerups
{
  public class ExtraLife : PowerUp
  {
    private ExtraLife(Vector2 position) : base()
    {
      Type = PowerUpTypes.ExtraLife;
      Position = position;
      var texture = Art.PowerUpLife;
      if (texture != null)
      {
        Radius = (float) (texture.Width / 2.0);
        Texture = texture;
      }
      DrawPriority = 1;
      CreationTime = DateTime.Now;
      PointValue = 5;
      ActiveDuration = 0;
    }

    protected override void ExpirationCallback()
    {
      Debug.WriteLine("Extralife expired....who cares");
    }

    public override void WasCaptured()
    {
      PlayerStatus.AddLife();
      Ship.Instance.NewLifeParticles();
      base.WasCaptured();
      CapturedPowerUpParticles(Color.Blue);

    }

    public override bool IsActive()
    {
      return false;
    }

    public static PowerUp Create(Vector2 position)
    {
      var powerUp = new ExtraLife(position);
      return powerUp;
    }
  }
}