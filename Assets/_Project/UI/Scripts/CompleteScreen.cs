using Mirror;
using TMPro;
using UnityEngine;

public class CompleteScreen : NetworkBehaviour
{
    [SerializeField] private TMP_Text _textWinnerNickname;

    public void SetTextWinnerNicknameText(string nickname)
    {
        _textWinnerNickname.text = nickname + " winned!";
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
