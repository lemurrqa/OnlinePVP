using Mirror;
using TMPro;
using UnityEngine;

public class ScoreInfoView : NetworkBehaviour
{
    [SerializeField] private TMP_Text _playerNicknameText;
    [SerializeField] private TMP_Text _playerScoreText;

    public void SetLocalPlayer()
    {
        _playerNicknameText.color = Color.green;
        _playerScoreText.color = Color.green;
    }

    public void OnPlayerNicknameChanged(string newPlayerNickname)
    {
        _playerNicknameText.text = newPlayerNickname;
    }

    public void OnPlayerScoreChanged(int score)
    {
        _playerScoreText.text = "Score: " + score;
    }
}
