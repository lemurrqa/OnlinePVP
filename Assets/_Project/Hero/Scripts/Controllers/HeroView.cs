using Mirror;
using System;

public class HeroView : NetworkBehaviour
{
    private ScoreInfoViewService _scoreViewService;
    private ScoreInfoView _scoreView;

    public Action<int> OnScoreChangedEvent;
    public Action<string> OnNicknameChangedEvent;

    [SyncVar(hook = nameof(OnNicknameChanged))]
    private string _nickname;

    public string Nickname => _nickname;

    private void Awake()
    {
        _scoreViewService = Mediator.GetScoreViewServiceFunc?.Invoke();
    }

    public override void OnStartClient()
    {
        _scoreView = _scoreViewService.SpawnAndGetInfoPanel();

        OnNicknameChangedEvent = _scoreView.OnPlayerNicknameChanged;
        OnScoreChangedEvent = _scoreView.OnPlayerScoreChanged;

        OnNicknameChangedEvent?.Invoke(_nickname);
    }

    public override void OnStartLocalPlayer()
    {
        if (_scoreView != null)
            _scoreView.SetLocalPlayer();
    }

    public override void OnStopClient()
    {
        OnNicknameChangedEvent = null;
        OnScoreChangedEvent = null;

        if (_scoreView != null)
        {
            Destroy(_scoreView.gameObject);
            NetworkServer.Destroy(_scoreView.gameObject);
        }
    }

    public void SetNickname(string nickname)
    {
        _nickname = nickname;
    }

    private void OnNicknameChanged(string oldName, string newName)
    {
        OnNicknameChangedEvent?.Invoke(_nickname);
    }
}
