namespace Asteroids
{
  public static class PlayerStatus
  {
    public static int Lives { get; private set; }
    public static int Score { get; private set; }
    public static bool IsGameOver => Lives == 0;
    private static int scoreForExtraLife;

    private const int MAX_LIFE_INTERVAL = 100;
    public const int MaxLives = 5;

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

    public static void RemoveLife()
    {
      Lives--;
    }

    public static void AddLife()
    {
      if (Lives < MaxLives)
        Lives++;
    }
  }
}