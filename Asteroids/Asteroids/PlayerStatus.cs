namespace Asteroids
{
  public static class PlayerStatus
  {
    public static int Lives { get; set; }
    public static int Score { get; set; }
    public static bool IsGameOver { get { return Lives == 0; } }
    private static int scoreForExtraLife;

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
      Lives = 4;
      scoreForExtraLife = 100;
    }

    public static void AddPoints(int basePoints)
    {
      if (Ship.Instance.IsDead) return;

      Score += basePoints;
      while (Score >= scoreForExtraLife)
      {
        scoreForExtraLife += 100;
        Lives++;
        Ship.Instance.NewLifeParticles();
      }
    }

    public static void RemoveLife()
    {
      Lives--;
    }

    public static void AddLife()
    {
      Lives++;
    }
  }
}