using Microsoft.Xna.Framework;

namespace Asteroids.TextEntities
{
  public class ScoreText : TextEntity
  {
    public ScoreText(Vector2 position) : base(position)
    {
      
    }

    protected override string GetText()
    {
      return $"Score: {PlayerStatus.Score}";
    }
  }
}