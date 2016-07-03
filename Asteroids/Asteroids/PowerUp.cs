using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
  public class PowerUp : Entity
  {
    public PowerUpTypes Type { get; private set; }
    private const int TIME_TO_LIVE = 10;
    private const int TIME_FOR_ACTIVE = 15;
    private readonly DateTime creationTime;
    private DateTime activeTime;
    public int PointValue { get; private set; }
    public bool Active { get; set; }
    public bool Expired { get; set; }
    private readonly Random rand=new Random();

    private PowerUp(Texture2D texture, Vector2 position, PowerUpTypes type)
    {
      Type = type;
      Position = position;
      if (texture != null)
      {
        Radius = texture.Width / 2;
        Texture = texture;
      }
      color = Color.White;
      DrawPriority = 1;
      creationTime = DateTime.Now;
      PointValue = 5;
    }

    public void WasCaptured()
    {
      Active = true;
      activeTime = DateTime.Now;
      Expired = false;
      ReadyToRemove = true;
      PlayerStatus.AddPoints(PointValue);

      capturedPowerUpParticles();
    }

    public static Entity CreateExtraLife(Vector2 position)
    {
      var powerup = new PowerUp(Art.PowerUpLife, position, PowerUpTypes.ExtraLife);
      return powerup;
    }

    public static Entity CreateMultiShoot(Vector2 position)
    {
      var powerup = new PowerUp(Art.PowerUpThreeWay, position, PowerUpTypes.MultiShoot);
      return powerup;
    }

    public override void Update()
    {
      if (!Active)
      {
        var elapsed = DateTime.Now - creationTime;
        if (elapsed > TimeSpan.FromSeconds(TIME_TO_LIVE))
        {
          uncapturedParticles();
          ReadyToRemove = true;
        }
      }
      else
      {
        var elapsed = DateTime.Now - activeTime;
        if (elapsed > TimeSpan.FromSeconds(TIME_FOR_ACTIVE))
          Expired = true;
      }
    }

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
  }
}