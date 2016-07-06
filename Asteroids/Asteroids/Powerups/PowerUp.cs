using System;
using Asteroids.Entities;
using Asteroids.TextEntities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Powerups
{
  public abstract class PowerUp : Entity
  {
    public PowerUpTypes Type { get; protected set; }
    protected int TTL = 10;
    protected int ActiveDuration = 15;
    protected DateTime CreationTime;
    private DateTime activeTime;
    protected int PointValue { get; set; }
    public bool Active { get; set; }
    public bool Expired { get; set; }
    private readonly Random rand=new Random();

    protected PowerUp()
    {
    }

//    protected PowerUp(Texture2D texture, Vector2 position, PowerUpTypes type, int pointValue, bool activeState)
//    {
//      Type = type;
//      Position = position;
//      if (texture != null)
//      {
//        Radius = texture.Width / 2;
//        Texture = texture;
//      }
//      color = Color.White;
//      DrawPriority = 1;
//      CreationTime = DateTime.Now;
//      PointValue = pointValue;
//      activeState = this.ActiveState;
//    }

    public virtual void WasCaptured()
    {
      Active = true;
      activeTime = DateTime.Now;
      Expired = false;
      ReadyToRemove = true;
      GameCore.TextManager.Add(new ActionScoreText(Position, PointValue.ToString()));
      PlayerStatus.AddPoints(PointValue);
      capturedPowerUpParticles();
    }

//    public abstract Entity Create(Vector2 position);

//    public static Entity CreateExtraLife(Vector2 position)
//    {
//      var powerup = new Extra(Art.PowerUpLife, position, PowerUpTypes.ExtraLife,5, false);
//      return powerup;
//    }
//
//    public static Entity CreateMultiShoot(Vector2 position)
//    {
//      var powerup = new PowerUp(Art.PowerUpThreeWay, position, PowerUpTypes.MultiShoot,3, true);
//      return powerup;
//
//    }
//    public static Entity CreateNuke(Vector2 position)
//    {
//      var powerup = new PowerUp(Art.PowerUpNuke, position, PowerUpTypes.Nuke,20, false);
//      return powerup;
//    }
//
//    public static Entity CreateShield(Vector2 position)
//    {
//      var powerup = new PowerUp(Art.PowerUpShield, position, PowerUpTypes.Shield,3, false);
//      return powerup;
//    }

    public override void Update()
    {
      if (!Active)
      {
        var elapsed = DateTime.Now - CreationTime;
        if (elapsed > TimeSpan.FromSeconds(TTL))
        {
          uncapturedParticles();
          ReadyToRemove = true;
        }
      }
      else
      {
        var elapsed = DateTime.Now - activeTime;
        if (elapsed > TimeSpan.FromSeconds(ActiveDuration))
        {
          ExpirationCallback();
          Expired = true;
        }
      }
    }

    protected abstract void ExpirationCallback();

    private void uncapturedParticles()
    {
      float hue1 = rand.NextFloat(0, 6);
      float hue2 = (hue1 + rand.NextFloat(0, 2)) % 6f;
      Color color1 = ColorUtil.HSVToColor(hue1, 0.5f, 1);
      Color color2 = ColorUtil.HSVToColor(hue2, 0.5f, 1);

      for (int i = 0; i < 120; i++)
      {
        float speed = 2 * (1f - 1 / rand.NextFloat(1f, 10f));
        var state = new ParticleState
        {
          Velocity = -rand.NextVector2(speed, speed),
          Type = ParticleType.EnemyExplosion,
          LengthMultiplier = 0.2f
        };
        
        Color particleColor = Color.Lerp(color1, color2, rand.NextFloat(0, 1));
        GameCore.ParticleManager.CreateParticle(Art.Glow, Position + rand.NextVector2(50f, 50f), particleColor, 1190, .1f, state);
      }
    }

    private void capturedPowerUpParticles()
    {
      float hue1 = rand.NextFloat(0, 6);
      float hue2 = (hue1 + rand.NextFloat(0, 2))%6f;
      Color color1 = ColorUtil.HSVToColor(hue1, 0.5f, 1);
      Color color2 = ColorUtil.HSVToColor(hue2, 0.5f, 1);

      for (int i = 0; i < 120; i++)
      {
        float speed = 2*(1f - 1/rand.NextFloat(1f, 10f));
        var state = new ParticleState
                      {
                        Velocity = rand.NextVector2(speed, speed),
                        Type = ParticleType.EnemyExplosion,
                        LengthMultiplier = 0.2f
                      };
        Color particleColor = Color.Lerp(color1, color2, rand.NextFloat(0, 1));
        GameCore.ParticleManager.CreateParticle(Art.Glow, Position + rand.NextVector2(2f, 3.3f), particleColor, 190, .1f, state);
      }
    }

    public abstract bool IsActive();
  }
}