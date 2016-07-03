using Microsoft.Xna.Framework;

namespace Asteroids.TextEntities
{
  public class PauseText : TextEntity
  {
    public PauseText(Vector2 position) : base(position)
    {
      Color = Color.Red;
    }

    protected override string GetText()
    {
      return "Game Paused";
    }
  }
}