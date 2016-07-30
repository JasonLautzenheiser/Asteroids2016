using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Asteroids
{
  public static class Font
  {
    public static BitmapFont MainFont { get; private set; }

    public static void Load(ContentManager content)
    {
      MainFont = content.Load<BitmapFont>(@"Arial24");
    }
  }
}