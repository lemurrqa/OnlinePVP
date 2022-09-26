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
                _hero.SetWinnedStatus();
            else
                _hero.CmdPlayerWinnedStatus();
        }
    }

    private void OnChangedScore(int oldValue, int newValue)
    {
        _hero.HeroUIController.OnScoreChangedEvent?.Invoke(_score);
    }
}
