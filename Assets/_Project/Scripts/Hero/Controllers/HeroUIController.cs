using Mirror;
using System;

public class HeroUIController : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnNicknameChanged))]
    private string Nickname;

    private PlayerInfoUI _heroInfoUI;
    private Hero _hero;

    public Action<int> OnScoreChangedEvent;
    public Action<string> OnNicknameChangedEvent;

    public void Init(Hero hero)
    {
        _hero = hero;
    }

    public void StartClient()
    {
        _heroInfoUI = SceneUIService.Instance.SpawnAndGetInfoPanel();

        OnNicknameChangedEvent = _heroInfoUI.OnPlayerNicknameChanged;
        OnScoreChangedEvent = _heroInfoUI.OnPlayerScoreChanged;

        OnNicknameChangedEvent?.Invoke(Nickname);
    }

    public void StartLocalPlayer()
    {
        if (_heroInfoUI != null)
            _heroInfoUI.SetLocalPlayer();
    }

    public void StopClient()
    {
        OnNicknameChangedEvent = null;
        OnScoreChangedEvent = null;

        if (_heroInfoUI != null)
        {
            Destroy(_heroInfoUI.gameObject);
            NetworkServer.Destroy(_heroInfoUI.gameObject);
        }
    }

    public void SetNickname(string nickname)
    {
        Nickname = nickname;
    }

    public string GetNickname()
    {
        return Nickname;
    }

    private void OnNicknameChanged(string oldName, string newName)
    {
        OnNicknameChangedEvent?.Invoke(Nickname);
    }
}
