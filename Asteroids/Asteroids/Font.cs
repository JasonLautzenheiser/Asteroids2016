using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
  public static class Font
  {
    public static SpriteFont Arial20 { get; private set; }

    public static void Load(ContentManager content)
    {
      Arial20 = content.Load<SpriteFont>(@"Fonts\asteroidsfont");
    }
  }
}