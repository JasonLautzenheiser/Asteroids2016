using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Managers;
using Asteroids.Powerups;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Asteroids.Entities.Player
{
  public class Ship : Entity
  {
    private static Ship instance;

    public static Ship Instance => instance ?? (instance = new Ship());

    private const float ANGLE180 = MathHelper.Pi;
    private const float MAX_ACCEL = 300f;
    private const float MAX_THRUST = 100f;
    private const float MAX_VELOCITY = 340f;
    private const float FRICTION_FACTOR = 0.9f;
    private const float SHIELD_LENGTH = 15f;
    private const float SHIELD_REGEN = 5f;

    private DateTime shieldStart;
    private DateTime shieldEnd;
    private int shieldUse = 0;
    private bool firstTimeShield = true;

    private float spin;
    private float thrust;
    private readonly Random rand;

    private DateTime lastLaserFire = DateTime.Now;

    private List<PowerUp> activePowerups = new List<PowerUp>();

    readonly SoundEffectInstance soundEffect;

    public double ShieldTimeLeft => (int)(SHIELD_LENGTH - (DateTime.Now - shieldStart).TotalSeconds);
    private KeyboardState lastState;

    private Ship()
    {
      Texture = Art.Player;
      Radius = 10;
      Rotation = 0;
      var soundLaser = SoundEffects.Laser;
      soundEffect = soundLaser.CreateInstance();
      rand = new Random();
      DrawPriority = 1;
      Mass = 1.0f;
      reset();
    }

    private void reset()
    {
      activePowerups.Clear();
      Position = GameCore.ScreenSize / 2;
      firstTimeShield = true;
    }

    private int framesUntilRespawn;
    public bool IsDead => framesUntilRespawn > 0;
    private bool areShieldsUp;
    public bool AreShieldsUp
    {
      get { return areShieldsUp; }
      private set 
      { 
        areShieldsUp = value;
        if (value)
          shieldStart = DateTime.Now;
        else
          shieldEnd = DateTime.Now;
      }
    }


    public override void Draw(SpriteBatch batch)
    {
      if (!IsDead)
        base.Draw(batch);
    }

    public override void Update()
    {
      if (IsDead)
      {
        if (--framesUntilRespawn != 0) return;
        if (PlayerStatus.Lives == 0)
        {
          PlayerStatus.Reset();
        }
        Position = GameCore.ScreenSize / 2;
        Velocity = Vector2.Zero;
        shieldUse = 0;
        return;
      }

      updateActivePowerups();

      if (GameCore.Instance.Window != null) this.WrapPositionIfCrossing(GameCore.Instance.Window.ClientBounds);

      if (InputManager.IsActionPressed(InputManager.Action.Fire))
      {
        if (FireLaser())
        {
          soundEffect.Volume = 0.2f;
          soundEffect.Play();
        }
      }

      // use nuke
      if (InputManager.IsActionTriggered(InputManager.Action.Nuke))
      {
        if (PlayerStatus.Nukes > 0)
        {
          PlayerStatus.Nukes--;
          nukeParticles();
          EntityManager.KillAllEnemies();
        }
      }

      // use shield
      if (InputManager.IsActionTriggered(InputManager.Action.Shield))
      {
        if (PlayerStatus.ShieldsLeft > 0)
          if (!AreShieldsUp)
          {
            var elapsed = DateTime.Now - shieldEnd;
            if (elapsed > TimeSpan.FromSeconds(SHIELD_REGEN) || firstTimeShield)
            {
              PlayerStatus.ShieldsLeft--;
              shieldStart = DateTime.Now;
              AreShieldsUp = true;
              firstTimeShield = false;
            }
          }
      }

      handleShields();
      handleThrust();
      handleRotation(GameCore.GameTime.ElapsedGameTime);
 
      Acceleration = (Math.Abs(thrust - 0) > 0.001f) ? applyThrust(thrust) : applyFriction();

      base.Update();

      clampVelocity();

    }

    private void nukeParticles()
    {
      Color explosionColor = new Color(0.8f, 0.8f, 0.4f);
      for (int i = 0; i < 1200; i++)
      {
        float speed = 38f; //18f * (1f - 1 / rand.NextFloat(1f, 10f));
        Color killColor = Color.Lerp(Color.Gold, explosionColor, rand.NextFloat(0, 1));
        var state = new ParticleState()
        {
          Velocity = rand.NextVector2(speed, speed),
          Type = ParticleType.None,
          LengthMultiplier = 1
        };
        GameCore.ParticleManager.CreateParticle(Art.LineParticle, Position, killColor, 190, 0.5f, state);
      }
    }

    private void updateActivePowerups()
    {
      foreach (var activePowerup in activePowerups)
      {
        if (activePowerup.Expired)
          removeExpiredPowerup();

        activePowerup.Update();
      }
    }

    private void removeExpiredPowerup()
    {
      activePowerups = activePowerups.Where(p => !p.Expired).ToList();
    }

    private void removeExtraLifePowerUp()
    {
      activePowerups = activePowerups.Where(p => p.Type != PowerUpTypes.ExtraLife).ToList();
    }

    private void handleShields()
    {
      if (!areShieldsUp) return;

      makeShields();
      var elapsed = DateTime.Now - shieldStart;
      if (elapsed > TimeSpan.FromSeconds(SHIELD_LENGTH))
        AreShieldsUp = false;
    }


    public void Kill()
    {
      PlayerStatus.RemoveLife();
      framesUntilRespawn = PlayerStatus.IsGameOver ? 300 : 120;

      Color explosionColor = new Color(0.8f,0.8f, 0.4f);
      for (int i = 0; i < 1200; i++)
      {
        float speed = 18f*(1f - 1/rand.NextFloat(1f, 10f));
        Color killColor = Color.Lerp(Color.White, explosionColor, rand.NextFloat(0, 1));
        var state = new ParticleState()
                      {
                        Velocity = rand.NextVector2(speed, speed),
                        Type = ParticleType.None,
                        LengthMultiplier = 1
                      };
        GameCore.ParticleManager.CreateParticle(Art.LineParticle, Position, killColor, 190, 1.5f, state);
      }

      reset();
    }


    private void makeShields()
    {
      for (int i=0; i<=2; i++)
      {
        float rads = (float) (rand.NextDouble()*MathHelper.TwoPi);
        Vector2 offset = new Vector2((float) Math.Cos(rads)*40, (float) Math.Sin(rads)*40);
        const float ALPHA = 0.7f;
        var shieldVelocity = rand.NextVector2(0, 1);
        GameCore.ParticleManager.CreateParticle(Art.LineParticle, Position + offset, Color.White * ALPHA, 60f, new Vector2(0.5f, 0.5f), new ParticleState(shieldVelocity, ParticleType.Shield));
        GameCore.ParticleManager.CreateParticle(Art.Glow, Position + offset, Color.Blue * ALPHA, 60f, new Vector2(1.5f, 1.5f), new ParticleState(shieldVelocity, ParticleType.Shield));
        GameCore.ParticleManager.CreateParticle(Art.Glow, Position + offset + rand.NextVector2(0, 1), Color.CadetBlue * ALPHA, 60f, 1.5f, new ParticleState(shieldVelocity, ParticleType.Shield));
        GameCore.ParticleManager.CreateParticle(Art.Glow, Position + offset + rand.NextVector2(0, 1), Color.Navy * ALPHA, 60f, 2.0f, new ParticleState(shieldVelocity, ParticleType.Shield));
        GameCore.ParticleManager.CreateParticle(Art.Glow, Position + offset + rand.NextVector2(0, 1), Color.Aqua * ALPHA, 60f, 1f, new ParticleState(shieldVelocity, ParticleType.Shield));
      }
    }

    private void makeExhaustFire()
    {
      if (Velocity.LengthSquared() > 0.1f)
      {
//        Rotation = Velocity.ToAngle();
        Quaternion rot = Quaternion.CreateFromYawPitchRoll(0f, 0f, Rotation);

        double t = GameCore.GameTime.TotalGameTime.TotalSeconds;
        Vector2 baseVel = Velocity.ScaleTo(-3);
        Vector2 perpVel = new Vector2(baseVel.Y, -baseVel.X) * (0.5f * (float)Math.Sin(t*10));
        Color sideColor = new Color(200,38,9);
        Color midColor =new Color(255,187,39);
        Vector2 pos = Position + Vector2.Transform(new Vector2(0, 0), rot);
        const float ALPHA = 0.7f;

        Vector2 velMid = baseVel + rand.NextVector2(0, 1);
        GameCore.ParticleManager.CreateParticle(Art.LineParticle, pos, Color.White *ALPHA, 60f, new Vector2(0.5f,1),new ParticleState(velMid, ParticleType.ShipExhaust) );
        GameCore.ParticleManager.CreateParticle(Art.Glow, pos, midColor*ALPHA, 60f, new Vector2(0.5f,1), new ParticleState(velMid, ParticleType.ShipExhaust));

        // side particle streams
        Vector2 vel1 = baseVel + perpVel + rand.NextVector2(0, 0.3f);
        Vector2 vel2 = baseVel - perpVel + rand.NextVector2(0, 0.3f);

        GameCore.ParticleManager.CreateParticle(Art.LineParticle, pos, Color.White * ALPHA, 60f, new Vector2(0.5f, 1),new ParticleState(vel1, ParticleType.ShipExhaust));
        GameCore.ParticleManager.CreateParticle(Art.LineParticle, pos, Color.White * ALPHA, 60f, new Vector2(0.5f, 1),new ParticleState(vel2, ParticleType.ShipExhaust));

        GameCore.ParticleManager.CreateParticle(Art.Glow, pos, sideColor * ALPHA, 60f, new Vector2(0.5f, 1),new ParticleState(vel1, ParticleType.ShipExhaust));
        GameCore.ParticleManager.CreateParticle(Art.Glow, pos, sideColor * ALPHA, 60f, new Vector2(0.5f, 1),new ParticleState(vel2, ParticleType.ShipExhaust));

      }

    }

    private void clampVelocity()
    {
      if (Velocity.Length() > MAX_VELOCITY)
        Velocity = Vector2.Normalize(Velocity)*MAX_VELOCITY;
    }

    private Vector2 applyFriction()
    {
      return (Velocity.Length() > 0 ? -Velocity*FRICTION_FACTOR : new Vector2(0, 0));
    }

    private Vector2 applyThrust(float f)
    {
      var tThrust = MAX_THRUST*f;

      var thrustDirection = new Vector2((float)(-tThrust * Math.Sin(Rotation - ANGLE180)), (float)(tThrust * Math.Cos(Rotation - ANGLE180)));
      var acc = new Vector2(Acceleration.X + thrustDirection.X, Acceleration.Y + thrustDirection.Y);

      return acc.Length() > MAX_ACCEL ? Vector2.Normalize(acc)*MAX_ACCEL : acc;
    }


    private void handleThrust()
    {
      if (InputManager.IsActionPressed(InputManager.Action.Thrust))
      {
        thrust += 0.05f;
        makeExhaustFire();
      }
      else if (InputManager.IsActionPressed(InputManager.Action.Brake))
      {
        thrust -= 0.05f;
        makeExhaustFire();
      }
      else
        thrust *= 0.1f;

      if (Math.Abs(thrust) < 0.01)
        thrust = 0;
      else
      {
        thrust = Math.Min(thrust, 1f);
        thrust = Math.Max(thrust, -1f);
      }
    }

    private void handleRotation( TimeSpan elapsedTime)
    {
      if (InputManager.IsActionPressed(InputManager.Action.RotateRight))
        spin -= 0.1f;
      else if (InputManager.IsActionPressed(InputManager.Action.RotateLeft))
        spin += 0.1f;
      else
        spin *= 0.9f;

      if (Math.Abs(spin) < 0.01)
        spin = 0;
      else
      {
        spin = Math.Min(spin, 4f);
        spin = Math.Max(spin, -4f);
      }

      Rotation -= (float) (spin*elapsedTime.TotalSeconds*3.0);
    }


    public bool FireLaser()
    {
      if (blasterNotReady()) return false;

      lastLaserFire = DateTime.Now;

      var trajectory = Vector2.Transform(new Vector2(0, 1), Matrix.CreateRotationZ(Rotation - MathHelper.Pi));
      var shot = new Laser(Position, trajectory);
      EntityManager.Add(shot);
      
      if (isMultiShootActive())
      {
        var trajectory1 = Vector2.Transform(new Vector2(0, 1), Matrix.CreateRotationZ(Rotation - (MathHelper.Pi-MathHelper.ToRadians(30))));
        var shot1 = new Laser(Position, trajectory1);
        EntityManager.Add(shot1);

        var trajectory2 = Vector2.Transform(new Vector2(0, 1), Matrix.CreateRotationZ(Rotation - (MathHelper.Pi-MathHelper.ToRadians(-30))));
        var shot2 = new Laser(Position, trajectory2);
        EntityManager.Add(shot2);
      }
      return true;
    }

    private bool isMultiShootActive()
    {
      return activePowerups.Any(p => p.Type == PowerUpTypes.MultiShoot);
    }

    private bool isExtraLifeActive()
    {
      return activePowerups.Any(p => p.Type == PowerUpTypes.ExtraLife);
    }

    private bool blasterNotReady()
    {
      var elapsed = (DateTime.Now - lastLaserFire);
      return elapsed < TimeSpan.FromSeconds(0.2);
    }

    public void AddPowerup(PowerUp powerUp)
    {
      activePowerups.Add(powerUp);     
    }

    public void NewLifeParticles()
    {
      for (int i = 0; i <= 26; i++)
      {
        float rads = (float)(rand.NextDouble() * MathHelper.TwoPi);
        Vector2 offset = new Vector2((float)Math.Cos(rads) * 20, (float)Math.Sin(rads) * 20);
        const float ALPHA = 1f;
        var shieldVelocity = rand.NextVector2(0, 1);
        GameCore.ParticleManager.CreateParticle(Art.LineParticle, Position + offset + rand.NextVector2(-2,2), Color.White * ALPHA, 60f, new Vector2(2.5f, 2.5f), new ParticleState(shieldVelocity, ParticleType.Shield));
        GameCore.ParticleManager.CreateParticle(Art.LineParticle, Position + offset + rand.NextVector2(-2, 2), Color.Yellow * ALPHA, 60f, new Vector2(2.5f, 2.5f), new ParticleState(shieldVelocity, ParticleType.Shield));
        GameCore.ParticleManager.CreateParticle(Art.LineParticle, Position + offset + rand.NextVector2(-2, 2), Color.Gold * ALPHA, 60f, new Vector2(2.5f, 2.5f), new ParticleState(shieldVelocity, ParticleType.Shield));
      }
    }

    public void NewShieldParticles()
    {
      for (int i = 0; i <= 26; i++)
      {
        float rads = (float)(rand.NextDouble() * MathHelper.TwoPi);
        Vector2 offset = new Vector2((float)Math.Cos(rads) * 20, (float)Math.Sin(rads) * 20);
        const float ALPHA = 1f;
        var shieldVelocity = rand.NextVector2(0, 1);
        GameCore.ParticleManager.CreateParticle(Art.LineParticle, Position + offset + rand.NextVector2(-2, 2), Color.Blue * ALPHA, 60f, new Vector2(2.5f, 2.5f), new ParticleState(shieldVelocity, ParticleType.Shield));
        GameCore.ParticleManager.CreateParticle(Art.LineParticle, Position + offset + rand.NextVector2(-2, 2), Color.Yellow * ALPHA, 60f, new Vector2(2.5f, 2.5f), new ParticleState(shieldVelocity, ParticleType.Shield));
        GameCore.ParticleManager.CreateParticle(Art.LineParticle, Position + offset + rand.NextVector2(-2, 2), Color.Gold * ALPHA, 60f, new Vector2(2.5f, 2.5f), new ParticleState(shieldVelocity, ParticleType.Shield));
      }
    }
  }
}