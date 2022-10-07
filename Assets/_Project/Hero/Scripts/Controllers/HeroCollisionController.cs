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
        if (!isLocalPlayer)
            return;

        if (!_hero.StateMachine.CheckActiveState(HeroStateType.UsingAbility))
            return;

        if (other.gameObject.TryGetComponent(out Hero targetHero))
        {
            if (targetHero.MaterialChanger.CanMaterialChange)
                return;

            _hero.HeroScore.CmdScoreIncrement();
            targetHero.MaterialChanger.CmdStartChange();
        }
    }
}
