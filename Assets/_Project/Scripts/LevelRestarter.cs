using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRestarter : NetworkBehaviour
{
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
        yield return new WaitForSeconds(5f);
        Restart();
    }
}
