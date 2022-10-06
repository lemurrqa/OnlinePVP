using Mirror;
using UnityEngine;
using Zenject;

public class LevelResultService : NetworkBehaviour
{
    [SerializeField] private CompleteScreen _completeScreen;

    [Inject] private LevelRestarter _levelRestarter;

    [SyncVar(hook = nameof(OnChangedWinnedNickname))]
    private string _winnedNickname;

    private void Start()
    {
        _completeScreen.Hide();
    }

    public void Complete(string nickname)
    {
        _winnedNickname = nickname;

        _levelRestarter.RestartLevel();
        RpcShowCompleteScreen();
    }

    [ClientRpc]
    private void RpcShowCompleteScreen()
    {
        _completeScreen.Show();
    }

    private void OnChangedWinnedNickname(string oldNick, string newNick)
    {
        _completeScreen.SetTextWinnerNicknameText(_winnedNickname);
    }
}