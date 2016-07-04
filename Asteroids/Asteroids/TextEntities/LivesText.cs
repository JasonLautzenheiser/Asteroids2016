using Microsoft.Xna.Framework;

namespace Asteroids.TextEntities
{
  public class LivesText : TextEntity
  {
    public LivesText(Vector2 position) : base(position)
    {
      Color = Color.GreenYellow;
    }

    protected override string GetText()
    {
      return $"Lives: {PlayerStatus.Lives} / {PlayerStatus.MaxLives}";
    }
  }
}