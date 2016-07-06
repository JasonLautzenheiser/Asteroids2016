using Asteroids.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Managers
{
  public interface IManager
  {
    void Add(Entity entity);
    void Draw(SpriteBatch batch);
    void Update();
  }
}