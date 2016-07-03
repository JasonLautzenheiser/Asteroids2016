using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Asteroids
{
  public class SoundEffects
  {
    public static SoundEffect Laser { get; private set; }
    public static SoundEffect ExplodeShip { get; private set; }
    public static SoundEffect ExplodeAsteroid { get; private set; }

    public static void Load(ContentManager content)
    {
      Laser = content.Load<SoundEffect>(@"Sounds\Laser_Shoot");
      ExplodeShip = content.Load<SoundEffect>(@"Sounds\ship_explode");
      ExplodeAsteroid = content.Load<SoundEffect>(@"Sounds\asteroid_explode");
    }
  }
}