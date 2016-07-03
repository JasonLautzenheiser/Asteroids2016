using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
  public abstract class Entity
  {
    public Texture2D Texture { get; set; }
    protected Color color = Color.White;
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 Acceleration { get; set; }
    protected Vector2 Origin = Vector2.Zero;
    public Vector2 Direction;
    public float Radius { get; set; }
    public float Rotation { get; set; }
    public virtual bool ReadyToRemove { get; set; }
    public float Mass { get; set; }
    public int DrawPriority { get; set; }

    public Vector2 Size
    {
      get { return Texture == null ? Vector2.Zero : new Vector2(Texture.Width, Texture.Height); }
    }


    protected Entity()
    {
      Acceleration = new Vector2();
      Position = new Vector2();
      Velocity = new Vector2();
    }

    public virtual void LoadContent()
    {
    }

    public virtual void Update()
    {
      var elapsedTime =  GameCore.GameTime.ElapsedGameTime;

      Velocity += Vector2.Multiply(Acceleration, (float) elapsedTime.TotalSeconds);
      Position += Vector2.Multiply(Velocity, (float) elapsedTime.TotalSeconds);
    }

    public virtual void Draw( SpriteBatch batch)
    {
      batch.Draw(Texture, Position, null,color,Rotation,Size/2f,1f,0,0);
    }

    public virtual Rectangle GetRectBounds(Texture2D texture)
    {
      return new Rectangle((int)Position.X-texture.Width/2, (int)Position.Y-texture.Height/2, texture.Width, texture.Height);
    }

    public virtual Rectangle GetBoundingRectangle()
    {
      return GetRectBounds(Texture);
    }
  }
}