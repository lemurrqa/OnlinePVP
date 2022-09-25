using Mirror;
using TMPro;
using UnityEngine;

public class CompleteScreen : NetworkBehaviour
{
    [SerializeField] private TMP_Text _textWinnedNickname;

    public TMP_Text TextWinnedNickname => _textWinnedNickname;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
