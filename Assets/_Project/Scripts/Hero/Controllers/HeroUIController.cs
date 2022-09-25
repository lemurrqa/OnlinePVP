using Mirror;
using TMPro;
using UnityEngine;

public class HeroUIController : NetworkBehaviour
{
    [SerializeField] private TMP_Text _textName;

    private Hero _hero;

    [SyncVar(hook = nameof(OnNicknameChanged))]
    public string Nickname;

    [SyncVar(hook = nameof(OnTextsRotationChanged))]
    private Vector3 TextsRotation;

    public void Init(Hero hero)
    {
        _hero = hero;
    }

    [Command]
    public void CmdSetName(string name)
    {
        Nickname = name;
    }

    public void SetRotationTextsFromCamera(Vector3 rotation)
    {
        TextsRotation = rotation;
    }

    private void OnTextsRotationChanged(Vector3 oldRotation, Vector3 newRotation)
    {
        _textName.transform.eulerAngles = TextsRotation;
    }

    private void OnNicknameChanged(string oldName, string newName)
    {
        _textName.text = Nickname;
        _hero.OnNicknameChangedEvent?.Invoke(Nickname);
    }
}
