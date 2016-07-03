using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
  public class Art
  {
    public static Texture2D Player { get; private set; }
    public static Texture2D Asteroid { get; private set; }
    public static Texture2D Bullet { get; private set; }
    public static Texture2D Background { get; private set; }
    public static Texture2D LineParticle { get; private set; }
    public static Texture2D Glow { get; private set; }
    public static Texture2D PowerUpLife { get; private set; }
    public static Texture2D PowerUpThreeWay { get; private set; }

    public static void Load(ContentManager content)
    {
      Player = content.Load<Texture2D>("ship2.png");
      Asteroid = content.Load<Texture2D>("rock7.png");
      Bullet = content.Load<Texture2D>("Blast.png");
      LineParticle = content.Load<Texture2D>("laser.png");
      Glow = content.Load<Texture2D>("Glow.png");
      PowerUpThreeWay = content.Load<Texture2D>("powerup_b");
      PowerUpLife = content.Load<Texture2D>("powerup_c");
      

      Background = new Texture2D(GameCore.Instance.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
      Background.SetData(new[] { Color.White });
    } 
  }
}