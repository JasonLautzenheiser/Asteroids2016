using Microsoft.Xna.Framework;

namespace Asteroids.TextEntities
{
  public class ActionScoreText : TextEntity
  {
    private readonly string text;
    public ActionScoreText(Vector2 position, string text)
    {
      this.text = text;
      TempText = true;
      TTL = 1.0;
      Position = position;
      Color = Color.Orange;
    }

    protected override string GetText()
    {
      return "+" + text;
    }
  }
}