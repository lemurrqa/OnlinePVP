using Mirror;
using TMPro;
using UnityEngine;

public class HeroUIController : NetworkBehaviour
{
    [SerializeField] private TMP_Text _textScore;

    private Hero _hero;

    public void Init(Hero hero)
    {
        _hero = hero;
    }

    public void RpcUpdateTextScore()
    {
        _textScore.text = _hero.HeroRoundController.Score.ToString();
    }
}
