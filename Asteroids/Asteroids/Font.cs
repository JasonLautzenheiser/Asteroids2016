using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
  public static class Font
  {
    public static SpriteFont MainFont { get; private set; }

    public static void Load(ContentManager content)
    {
      MainFont = content.Load<SpriteFont>(@"Fonts\Calibri-14");
    }
  }
}