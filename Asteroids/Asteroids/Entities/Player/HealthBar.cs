using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Shapes;

namespace Asteroids.Entities.Player
{
  public class HealthBar : Entity
  {
    private Texture2D bar;
    private float startBar = GameCore.Instance.GraphicsDevice.Viewport.Width/2.0f-250;

    private int currentHealth;

    public HealthBar()
    {
      Position = new Vector2(GameCore.Instance.GraphicsDevice.Viewport.Width/2.0f, 10);
      bar = new Texture2D(GameCore.Instance.GraphicsDevice, 2, 20);
      Color[] data = new Color[2*20];
      for (int pixel = 0; pixel < data.Length; pixel++)
      {
        data[pixel] = Color.White;
      }
      bar.SetData(data);
    }

    public override void Update()
    {
      if (Ship.Instance.VisibleHealth > Ship.Instance.CurrentHealth)
        Ship.Instance.VisibleHealth -= 1.0f;

      if (Ship.Instance.VisibleHealth < Ship.Instance.CurrentHealth)
        Ship.Instance.VisibleHealth += 1.0f;
    }

    public override void Draw(SpriteBatch batch)
    {
      var nameSize = Font.MainFont.MeasureString("Health");

      batch.DrawString(Font.MainFont, "Health", new Vector2(startBar - nameSize.Width, 10), Color.MediumAquamarine);
      batch.Draw(bar, new RectangleF(startBar,10f,500f,20f).ToRectangle(),Color.DarkGray);

      var percentHealth = (Ship.Instance.VisibleHealth / Ship.Instance.MaxHealth);
      Color barColor = Color.DarkGreen;
      if (percentHealth < 0.25f)
        barColor = Color.DarkRed;
      else if (percentHealth < 0.50f)
        barColor = Color.Goldenrod;
      
      batch.Draw(bar, new RectangleF(startBar, 10f, percentHealth * 500f, 20f).ToRectangle(), barColor);
    }
  }
}