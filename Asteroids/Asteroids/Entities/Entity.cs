using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Entities
{
  public abstract class Entity
  {
    protected Color color = Color.White;
    public Vector2 Direction;
    protected Vector2 Origin = Vector2.Zero;


    protected Entity()
    {
      Acceleration = new Vector2();
      Position = new Vector2();
      Velocity = new Vector2();
    }

    public Texture2D Texture { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 Acceleration { get; set; }
    public float Radius { get; set; }
    public float Rotation { get; set; }
    public virtual bool ReadyToRemove { get; set; }
    public float Mass { get; set; }
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public int DrawPriority { get; set; }
    public int Damage { get; set; }

    public Vector2 Size => Texture == null ? Vector2.Zero : new Vector2(Texture.Width, Texture.Height);

    public virtual void LoadContent()
    {
    }

    public virtual void Update()
    {
      var elapsedTime = GameCore.GameTime.ElapsedGameTime;

      Velocity += Vector2.Multiply(Acceleration, (float) elapsedTime.TotalSeconds);
      Position += Vector2.Multiply(Velocity, (float) elapsedTime.TotalSeconds);
    }

    public virtual void Draw(SpriteBatch batch)
    {
      batch.Draw(Texture, Position, null, color, Rotation, Size/2f, 1f, 0, 0);
    }

    public virtual Rectangle GetRectBounds(Texture2D texture)
    {
      return new Rectangle((int) Position.X - texture.Width/2, (int) Position.Y - texture.Height/2, texture.Width, texture.Height);
    }

    public virtual Rectangle GetBoundingRectangle()
    {
      return GetRectBounds(Texture);
    }

    public virtual void Die()
    {
      ReadyToRemove = true;
    }
  }
}