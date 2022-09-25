using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUIController : Singleton<SceneUIController>
{
    [SerializeField] private PlayerInfoUI _heroInfoTemplate;
    [SerializeField] private Transform _heroInfoParent;
    [SerializeField] private CompleteScreen _completeScreen;

    private NetworkIdentity[] _networkIdents;

    [SyncVar(hook = nameof(OnChangedWinnedNickname))]
    public string WinnedNickname;

    private void Start()
    {
        _completeScreen.Hide();
    }

    public PlayerInfoUI SpawnAndGetInfoPanel()
    {
        var playerUIObject = Instantiate(_heroInfoTemplate, _heroInfoParent);
        NetworkServer.Spawn(playerUIObject.gameObject);
        return playerUIObject;
    }

    public void CompleteRound(string winnedNickname)
    {
        WinnedNickname = winnedNickname;

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

    private void OnChangedWinnedNickname(string oldNick, string newNick)
    {
        _completeScreen.TextWinnedNickname.text = WinnedNickname + " winned!";
    }

    private IEnumerator HideCompleteScreenRoutine()
    {
        yield return new WaitForSeconds(5f);
        Restart();
    }
}
