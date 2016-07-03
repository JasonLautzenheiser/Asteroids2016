using Microsoft.Xna.Framework;

namespace Asteroids
{
  public static class GameObjectExtension
  {
    public static void WrapPositionIfCrossing(this Entity item, Rectangle bounds)
    {
      var w = bounds.Width + 2 * item.Radius;
      var h = bounds.Height + 2 * item.Radius;

      var x = item.Position.X;
      var y = item.Position.Y;

      if (x > w)
        x = -item.Radius;
      else if (x < -item.Radius)
        x = w;

      if (y > h)
        y = -item.Radius;
      else if (y < -item.Radius)
        y = h;

      item.Position = new Vector2(x, y);
    }
  }
}