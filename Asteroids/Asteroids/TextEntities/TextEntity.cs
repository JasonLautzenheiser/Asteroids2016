using System;
using Asteroids.Entities;
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

    private float mFadeDelay = 0.035f;
    private float mAlphaValue = 1.0f;
    private float mFadeIncrement = 0.05f;

    public override void Draw(SpriteBatch batch) => batch.DrawString(Font, GetText(), Position, Color * mAlphaValue);

    public override void Update()
    {
      if (TempText)
      {
        mFadeDelay -= (float)GameCore.GameTime.ElapsedGameTime.TotalSeconds;
        Position -= new Vector2(0,1) * 2.0f;

        if (mFadeDelay <= 0)
        {
          mFadeDelay = 0.035f;
          mAlphaValue -= mFadeIncrement;
        }
      }
      base.Update();
    }

    protected abstract string GetText();
  }
}