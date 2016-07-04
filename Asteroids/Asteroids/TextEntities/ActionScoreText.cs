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
      TTL = 3.0;
      Position = position;
      Color = Color.Yellow;
    }

    protected override string GetText()
    {
      return "+" + text;
    }
  }
}