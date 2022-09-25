using Mirror;
using UnityEngine;

public class HeroCollisionController : NetworkBehaviour
{
    private Hero _hero;

    //[SyncVar]
    //private bool _canCheckCollision = false;

    //private void Update()
    //{
    //    if (!_canCheckCollision)
    //        return;

    //    RaycastHit hit = new RaycastHit();

    //    var layerMask = LayerMask.GetMask("Hero");

    //    var startPos = new Vector3(_hero.transform.position.x, _hero.transform.position.y + 1f, _hero.transform.position.z);
    //    var rayPosRight = new Vector3(_hero.transform.forward.x + 0.5f, _hero.transform.forward.y * 1.2f, _hero.transform.forward.z * 1.2f);
    //    var rayPosLeft = new Vector3(_hero.transform.forward.x - 0.5f, _hero.transform.forward.y * 1.2f, _hero.transform.forward.z * 1.2f);

    //    if (Physics.Raycast(startPos, _hero.transform.forward * 1.3f, out hit, 2f, layerMask))
    //    {
    //        var findedHero = hit.transform.GetComponent<HeroHelperForRaycast>().Hero;

    //        if (CheckAndHandleCollision(findedHero))
    //            return;
    //    }

    //    if (Physics.Raycast(startPos, rayPosRight, out hit, 2f, layerMask))
    //    {
    //        var findedHero = hit.transform.GetComponent<HeroHelperForRaycast>().Hero;

    //        if (CheckAndHandleCollision(findedHero))
    //            return;
    //    }

    //    if (Physics.Raycast(startPos, rayPosLeft, out hit, 2f, layerMask))
    //    {
    //        var findedHero = hit.transform.GetComponent<HeroHelperForRaycast>().Hero;

    //        if (CheckAndHandleCollision(findedHero))
    //            return;
    //    }

    //    _canCheckCollision = false;
    //}

    //public void CanCheckCollision(bool canCheck)
    //{
    //    _canCheckCollision = canCheck;
    //}

    //[Command]
    //public void CmdCanCheckCollision(bool canCheck)
    //{
    //    CanCheckCollision(true);
    //}

    //private bool CheckAndHandleCollision(Hero findedHero)
    //{
    //    if (findedHero != null)
    //    {
    //        if (findedHero.IsInvulnerable)
    //            return false;

    //        if (_hero.isServer)
    //            _hero.ScoreIncrement();
    //        else
    //            _hero.CmdScoreIncrement();

    //        //if (findedHero.isServer)
    //        //{
    //        //    findedHero.SetInvulnerable(true);
    //        //}
    //        //else
    //        //{
    //        //    findedHero.CmdSetInvulnerable(true);
    //        //}

    //        if (findedHero.isServer)
    //            findedHero.ColorChanger.ChangeColor();
    //        else
    //            findedHero.ColorChanger.CmdChangeColor();

    //        _canCheckCollision = false;
    //        return true;
    //    }

    //    return false;
    //}

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

        if (other.gameObject.TryGetComponent(out Hero hero))
        {
            if (_hero.isServer)
                _hero.ScoreIncrement();
            else
                _hero.CmdScoreIncrement();

            if (hero.isServer)
                hero.ColorChanger.ChangeColor();
            else
                hero.ColorChanger.CmdChangeColor();
        }
    }
}
