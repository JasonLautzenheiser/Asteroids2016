using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.TextEntities
{
  public abstract class TextEntity : Entity
  {
    protected TextEntity()
    {
    }

    protected TextEntity(Vector2 position)
    {
      Position = position;
    }

    protected Color Color = Color.White;
    protected SpriteFont Font = Asteroids.Font.MainFont;
    protected Vector2 Position { get; set; }
    public bool TempText { get; set; }
    protected internal double TTL = 1.0;
    public DateTime DateOfCreation { get; set; } = DateTime.Now;
    public override bool ReadyToRemove { get; set; }

    public override void Draw(SpriteBatch batch) => batch.DrawString(Font, GetText(), Position, Color);

    protected abstract string GetText();
  }
}