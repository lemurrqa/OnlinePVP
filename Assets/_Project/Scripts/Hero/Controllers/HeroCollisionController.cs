using Mirror;
using UnityEngine;

public class HeroCollisionController : NetworkBehaviour
{
    private Hero _hero;

    public void Init(Hero hero)
    {
        _hero = hero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_hero.IsUsedAbility)
            return;

        if (!isLocalPlayer)
            return;

        if (other.gameObject.TryGetComponent(out Hero targetHero))
        {
            if (targetHero.IsInvulnerable)
                return;

            if (_hero.isServer)
            {
                _hero.ScoreIncrement();
            }
            else
            {
                _hero.CmdScoreIncrement();

            }

            if (targetHero.isServer)
            {
                targetHero.ChangeColor();
            }
            else
            {
                targetHero.CmdChangeColor();
            }

        }
    }
}
