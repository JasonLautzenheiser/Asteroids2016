using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
  public static class Art
  {
    public static Texture2D Player { get; private set; }
    public static Texture2D Asteroid { get; private set; }
    public static Texture2D Seeker { get; private set; }
    public static Texture2D SeekerShot { get; private set; }
    public static Texture2D SmallAsteroid { get; private set; }
    public static Texture2D Bullet { get; private set; }
    public static Texture2D Background { get; private set; }
    public static Texture2D LineParticle { get; private set; }
    public static Texture2D Glow { get; private set; }
    public static Texture2D PowerUpLife { get; private set; }
    public static Texture2D PowerUpThreeWay { get; private set; }
    public static Texture2D PowerUpNuke { get; private set; }
    public static Texture2D PowerUpShield { get; private set; }

    public static void Load(ContentManager content)
    {
      Player = content.Load<Texture2D>("ship");
      Asteroid = content.Load<Texture2D>("rock");
      Seeker = content.Load<Texture2D>("e_f1");
      SeekerShot = content.Load<Texture2D>("laser");
      
      SmallAsteroid = content.Load<Texture2D>("rockSmall");
      Bullet = content.Load<Texture2D>("Blast");
      LineParticle = content.Load<Texture2D>("laser");
      Glow = content.Load<Texture2D>("Glow");
      PowerUpThreeWay = content.Load<Texture2D>(@"Powerups\multishot-pu");
      PowerUpLife = content.Load<Texture2D>(@"Powerups\extralife-pu");
      PowerUpNuke = content.Load<Texture2D>(@"Powerups\nuke-pu");
      PowerUpShield = content.Load<Texture2D>(@"Powerups\shield-pu");

      Background = new Texture2D(GameCore.Instance.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
      Background.SetData(new[] { Color.White });
    } 
  }
}