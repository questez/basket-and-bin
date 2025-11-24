public class Player
{
    private int score = 0;

    public void IncreaseScore()
    {
        score++;
    }

    public int GetScore() => score;

    public void ResetScore()
    {
        score = 0;
    }
}
