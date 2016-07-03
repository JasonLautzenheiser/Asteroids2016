using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
  public class ParticleManager<T>
  {
    private Action<Particle> updateParticle;
    private CircularParticleArray particleList;

    public ParticleManager(int capacity, Action<Particle> updateParticle)
    {
      this.updateParticle = updateParticle;
      particleList = new CircularParticleArray(capacity);

      for (int i = 0; i < capacity; i++)
        particleList[i] = new Particle();
    }

    public void CreateParticle(Texture2D texture, Vector2 position, Color tint, float duration, float scale, T state, float theta = 0)
    {
      CreateParticle(texture, position, tint, duration, new Vector2(scale), state, theta);
    }

    public void CreateParticle(Texture2D texture, Vector2 position, Color tint, float duration, Vector2 scale, T state, float theta = 0)
    {
      Particle particle;
      if (particleList.Count == particleList.Capacity)
      {
        particle = particleList[0];
        particleList.Start++;
      }
      else
      {
        particle = particleList[particleList.Count];
        particleList.Count++;
      }

      particle.Texture = texture;
      particle.Position = position;
      particle.Tint = tint;
      particle.Duration = duration;
      particle.PercentLife = 1f;
      particle.Scale = scale;
      particle.Rotation = theta;
      particle.State = state;
    }

    public void Update()
    {
      int removalCount = 0;
      for (int i = 0; i < particleList.Count; i++)
      {
        var particle = particleList[i];
        updateParticle(particle);
        particle.PercentLife -= 1f/particle.Duration;

        swap(particleList, i - removalCount, i);
        if (particle.PercentLife < 0)
          removalCount++;
      }
      particleList.Count -= removalCount;
    }

    private static void swap(CircularParticleArray list, int index1, int index2)
    {
      var temp = list[index1];
      list[index1] = list[index2];
      list[index2] = temp;
    }

    public void Draw(SpriteBatch batch)
    {
      batch.DrawString(Font.MainFont,string.Format("Particle Count:{0}", particleList.Count),new Vector2(10,50),Color.Green );

      for (int i=0;i<particleList.Count; i++)
      {
        var particle = particleList[i];
        var origin = new Vector2(particle.Texture.Width/2, particle.Texture.Height/2);
        batch.Draw(particle.Texture, particle.Position, null, particle.Tint, particle.Rotation,origin, particle.Scale, 0,0 );
      }
    }

    public class Particle
     {
       public Texture2D Texture;
       public Vector2 Position;
       public float Rotation;
       public Vector2 Scale = Vector2.One;
       public Color Tint;
       public float Duration;
       public float PercentLife = 1f;
       public T State;
     }

    private class CircularParticleArray
    {
      private int start;
      public int Start
      {
        get { return start; }
        set { start = value%list.Length; }
      }
      public int Count { get; set; }
      public int Capacity { get { return list.Length; } }
      private Particle[] list;

      public CircularParticleArray(int capacity)
      {
        list = new Particle[capacity];
      }

      public Particle this[int i]
      {
        get { return list[(start + i)%list.Length]; }
        set { list[(start + i)%list.Length] = value; }
      }
    }
  }
}