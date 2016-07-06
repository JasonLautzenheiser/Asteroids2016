using Asteroids.Entities;
using Asteroids.Entities.Player;
using Microsoft.Xna.Framework;

namespace Asteroids.TextEntities
{
  public class ShieldsText :TextEntity
  {
    public ShieldsText(Vector2 position) : base(position)
    {
      Color = Color.AliceBlue;
    }

    protected override string GetText()
    {
      return displayTextForShields();
    }

    private string displayTextForShields()
    {
      var s = Ship.Instance.AreShieldsUp ? $"Shields Left: {Ship.Instance.ShieldTimeLeft} secs" : $"Shields: {Ship.Instance.ShieldsLeft}";
      return s;
    }
  }
}