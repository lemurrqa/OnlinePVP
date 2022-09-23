using Mirror;

public class HeroRoundController
{
    private Hero _hero;

    [SyncVar(hook = "UpdateRoundCounter")]
    private int _score = 0;

    public int Score
    {
        get => _score;
        private set => _score = value;
    }

    public HeroRoundController(Hero hero)
    {
        _hero = hero;
        _score = 0;
    }

    public void RoundCounterIncrement()
    {
        Score++;

        _hero.HeroUIController.RpcUpdateTextScore();

        if (Score >= 3)
        {

        }
    }

    private void UpdateRoundCounter(int oldScore, int newScore)
    {
        _score = newScore;
    }
}
