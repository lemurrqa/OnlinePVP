using Mirror;
using UnityEngine;

public class HeroCollisionController : NetworkBehaviour
{
    private Hero _hero;
    private NetworkRoomManagerCustom _networkRoomManagerCustom;

    public void Init(Hero hero)
    {
        _hero = hero;
        _networkRoomManagerCustom = NetworkManager.singleton as NetworkRoomManagerCustom;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isLocalPlayer)
            return;

        if (!_hero.StateMachine.CheckActiveState(HeroStateType.UsingAbility))
            return;

        if (other.gameObject.TryGetComponent(out Hero targetHero))
        {
            if (targetHero.MaterialChanger.IsMaterialChange)
                return;

            _hero.ScoreController.CmdScoreIncrement();
            targetHero.MaterialChanger.CmdStartChange();

            //if (_hero.isServer)
            //    _hero.ScoreIncrement();
            //else
            //    _hero.CmdScoreIncrement();

            //if (targetHero.isServer)
            //    targetHero.ChangeColor();
            //else
            //    targetHero.CmdChangeColor();


            //if (targetHero.isServer)
            //    targetHero.StateMachine.Enter<HeroStateDamagable>();
            //else
            //    targetHero.CmdSetStateDamagable();
        }
    }
}
