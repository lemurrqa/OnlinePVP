using Mirror;

public class HeroScoreController : NetworkBehaviour
{
    private Hero _hero;

    [SyncVar(hook = nameof(OnChangedScore))]
    private int _score = 0;

    public void Init(Hero hero)
    {
        _hero = hero;
    }

    public void ResetScore()
    {
        _score = 0;
    }

    public void ScoreIncrement()
    {
        _score++;

        if (_score >= 3)
        {
            _score = 3;

            if (_hero.isServer)
                _hero.SetWinnedStatus(true);
            else
                _hero.CmdPlayerWinnedStatus(true);
        }
    }

    private void OnChangedScore(int oldScore, int newScore)
    {
        _hero.OnScoreChangedEvent?.Invoke(_score);
    }
}
