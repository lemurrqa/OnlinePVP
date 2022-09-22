using Mirror;
using UnityEngine;

public class HeroRoundController : NetworkBehaviour
{
    private int _roundCounter;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        _roundCounter = 0;
    }

    public void RoundCounterIncrement()
    {
        Debug.Log(netId);
        _roundCounter++;

        if (_roundCounter >= 3)
        {
            Debug.Log("Win:" + name);
            Time.timeScale = 0f;
        }
    }
}
