public class Player
{
    private static int score;

    public static void IncreaseScore()
    {
        score++;
    }

    public static int GetScore() => score;

    public static void ResetScore()
    {
        score = 0;
    }
}
