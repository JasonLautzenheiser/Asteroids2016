using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
  public class Starfield : Entity
  {
//    private readonly GraphicsDevice  graphicsDevice;
    private Vector2[] stars;
    private Texture2D starTexture;
    private Vector2 position;
//    private Game game;
    private const int NUMBER_OF_STARS = 175;

    public Starfield(int numberOfStars)
    {
      init(numberOfStars);
    }
    public Starfield()
    {
      init(NUMBER_OF_STARS);
    }

    private void init(int numberOfStars)
    {
      DrawPriority = 2;
      stars = new Vector2[numberOfStars];
      createStars(position);

    }

    private void createStars(Vector2 pPosition)
    {


      int viewWidth = GameCore.Viewport.Width;
      int viewHeight = GameCore.Viewport.Height;
      var rnd = new Random();

      for (int i = 0; i < stars.Length; ++i)
      {
        stars[i] = new Vector2(rnd.Next(0,viewWidth),rnd.Next(0,viewHeight));
      }
      position = pPosition;
    }

    public override void Draw(SpriteBatch batch)
    {
      // draw background of starfield
//      batch.Draw(Art.Background, new Rectangle(0, 0, GameCore.Viewport.Width, GameCore.Viewport.Height), Color.Black);

      // draw the stars
      for (int i = 0; i < stars.Length; i++)
      {
        batch.Draw(Art.Background, new Rectangle((int)stars[i].X, (int)stars[i].Y, 2, 2), null, Color.Gray);
      }
    }

  }
}