using Mirror;
using UnityEngine;

public class HeroCollisionController
{
    private Hero _hero;
    [SyncVar] private bool _canCheckCollision = true;

    public HeroCollisionController(Hero hero)
    {
        _hero = hero;
    }

    [Command]
    public void CmdSetCheckCollision(bool canCheck)
    {
        _canCheckCollision = canCheck;
    }

    public void CollisionCheck()
    {
        if (!_canCheckCollision)
            return;

        RaycastHit hit;

        var startPos = new Vector3(_hero.transform.position.x, _hero.transform.position.y + 1f, _hero.transform.position.z);

        if (Physics.Raycast(startPos, _hero.transform.forward * 1.3f, out hit, 2.5f, LayerMask.GetMask("Hero")))
        {
            var findedPlayer = hit.transform.GetComponent<HeroHelperForRaycast>().Hero;

            if (findedPlayer != null)
            {
                if (findedPlayer.IsInvulnerable)
                    return;

                _hero.HeroRoundController.RoundCounterIncrement();
                findedPlayer.HeroColorChanger.ChangeColorFromDamage();

                _canCheckCollision = false;
                return;
            }
        }
    }
}
