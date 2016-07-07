using Microsoft.Xna.Framework;

namespace Asteroids.TextEntities
{
  public class GenericText : TextEntity
  {
    private readonly string text2Print;
    public GenericText(string name, Vector2 position, string text, Color color) : base(position)
    {
      Color = color;
      text2Print = text;
      Name = name;
    }

    protected override string GetText()
    {
      return text2Print;
    }
  }
}