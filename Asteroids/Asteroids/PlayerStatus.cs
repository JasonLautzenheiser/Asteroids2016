using Asteroids.Entities;
using Asteroids.Entities.Player;

namespace Asteroids
{
  public static class PlayerStatus
  {
    public static int Lives { get; private set; }
    public static int Score { get; private set; }
    public static bool IsGameOver => Lives == 0;
    public static bool GodMode { get; set; } = false;


    private static int scoreForExtraLife;

    private static int MAX_LIFE_INTERVAL = 100;
    public static int MaxLives = 5;

    public static int Nukes = 0;
    public static int MaxNukes = 2;

    public static int ShieldsPerLife = 3;
    public static int ShieldsLeft { get; set; } = ShieldsPerLife;

    static PlayerStatus()
    {
      Reset();
    }

    public static void Update()
    {
      
    }

    public static void Reset()
    {
      Score = 0;
      Lives = MaxLives;
      scoreForExtraLife = MAX_LIFE_INTERVAL;
      ShieldsLeft = ShieldsPerLife;
      Nukes = 0;
    }

    public static void AddPoints(int basePoints)
    {
      if (Ship.Instance.IsDead) return;

      Score += basePoints;
      while (Score >= scoreForExtraLife)
      {
        scoreForExtraLife += MAX_LIFE_INTERVAL;
        AddLife();
        Ship.Instance.NewLifeParticles();
      }
    }

    public static void AddNuke()
    {
      if (Nukes < MaxNukes)
        Nukes++;
    }

    public static void RemoveLife()
    {
      Lives--;
    }

    public static void AddLife()
    {
      if (Lives < MaxLives)
        Lives++;
    }

    public static void AddShield()
    {
      if (ShieldsLeft < ShieldsPerLife)
        ShieldsLeft++;
    }
  }
}