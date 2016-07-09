using Microsoft.Xna.Framework;

namespace Asteroids.TextEntities
{
  public class NukeText :TextEntity
  {
    public NukeText(Vector2 position) : base(position)
    {
      Color = Color.Gold;
    }

    protected override string GetText()
    {
      var s = PlayerStatus.Nukes > 0 ? $"Nukes Available: {PlayerStatus.Nukes}" : "Nuke Bay Empty";
      return s;
    }
  }
}