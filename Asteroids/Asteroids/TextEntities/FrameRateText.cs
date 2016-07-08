using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Asteroids.TextEntities
{
  public class FrameRateText : TextEntity
  {
    private long totalFrames { get; set; }
    private float totalSeconds { get; set; }
    private float averageFramesPerSecond { get; set; }
    private float currentFramesPerSecond { get; set; }

    private const int MAXIMUM_SAMPLES = 100;

    private readonly Queue<float> sampleBuffer = new Queue<float>();


    public FrameRateText(Vector2 position) : base(position)
    {
      Position = position;
      Color = Color.ForestGreen;
    }

    protected override string GetText()
    {
      return $"FPS: {averageFramesPerSecond.ToString("F1")}";
    }

    public override void Update()
    {
      float deltaTime = (float)GameCore.GameTime.ElapsedGameTime.TotalSeconds;
      currentFramesPerSecond = 1.0f / deltaTime;

      sampleBuffer.Enqueue(currentFramesPerSecond);

      if (sampleBuffer.Count > MAXIMUM_SAMPLES)
      {
        sampleBuffer.Dequeue();
        averageFramesPerSecond = sampleBuffer.Average(i => i);
      }
      else
      {
        averageFramesPerSecond = currentFramesPerSecond;
      }

      totalFrames++;
      totalSeconds += deltaTime;
//      base.Update();
    }
  }
}