using Mirror;
using Zenject;

public class LevelResultService : NetworkBehaviour
{
    [Inject] private Mediator _mediator;

    [SyncVar(hook = nameof(OnChangedWinnedNickname))]
    private string _winnedNickname;

    private void Start()
    {
        _mediator.CompleteScreen.Hide();
    }

    public void Complete(string nickname)
    {
        _winnedNickname = nickname;

        _mediator.RestartLevel();

        RpcShowCompleteScreen();
    }

    [ClientRpc]
    private void RpcShowCompleteScreen()
    {
        _mediator.CompleteScreen.Show();
    }

    private void OnChangedWinnedNickname(string oldNick, string newNick)
    {
        _mediator.CompleteScreen.SetTextWinnerNicknameText(_winnedNickname);
    }
}