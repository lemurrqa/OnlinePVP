using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restarter : NetworkBehaviour
{
    [SerializeField] private float _secondsBeforeRestart = 5f;

    public void RestartLevel()
    {
        StartCoroutine(RestartRoutine());
    }

    [Server]
    private void Restart()
    {
        NetworkManager.singleton.ServerChangeScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator RestartRoutine()
    {
        yield return new WaitForSeconds(_secondsBeforeRestart);
        Restart();
    }
}
