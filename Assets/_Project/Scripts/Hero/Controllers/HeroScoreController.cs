using Mirror;

public class HeroScoreController : NetworkBehaviour
{
    private Hero _hero;

    [SyncVar(hook = nameof(OnChangedScore))]
    public int Score = 0;

    public void Init(Hero hero)
    {
        _hero = hero;
    }

    public void ResetScore()
    {
        Score = 0;
    }

    public void ScoreIncrement()
    {
        Score++;

        if (Score >= 3)
        {
            Score = 3;

            if (_hero.isServer)
                _hero.SetWinnedStatus(true);
            else
                _hero.CmdPlayerWinnedStatus(true);
        }
    }

    private void OnChangedScore(int oldScore, int newScore)
    {
        _hero.OnScoreChangedEvent?.Invoke(Score);
    }
}
