using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUIService : Singleton<SceneUIService>
{
    [SerializeField] private PlayerInfoUI _heroInfoTemplate;
    [SerializeField] private CompleteScreen _completeScreen;
    [SerializeField] private Transform _heroInfoParent;

    [SyncVar(hook = nameof(OnChangedWinnedNickname))]
    private string _winnedNickname;

    private void Start()
    {
        _completeScreen.Hide();
    }

    public PlayerInfoUI SpawnAndGetInfoPanel()
    {
        var playerUIObject = Instantiate(_heroInfoTemplate, _heroInfoParent);
        return playerUIObject;
    }

    public void CompleteRound(string winnedNickname)
    {
        _winnedNickname = winnedNickname;

        StartCoroutine(HideCompleteScreenRoutine());
        RpcShowCompleteScreen();
    }

    [ClientRpc]
    private void RpcShowCompleteScreen()
    {
        _completeScreen.Show();
    }

    [Server]
    private void Restart()
    {
        NetworkManager.singleton.ServerChangeScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator HideCompleteScreenRoutine()
    {
        yield return new WaitForSeconds(5f);
        Restart();
    }

    private void OnChangedWinnedNickname(string oldNick, string newNick)
    {
        _completeScreen.TextWinnedNickname.text = _winnedNickname + " winned!";
    }
}
