using Mirror;
using UnityEngine;

public class HeroCollisionController : NetworkBehaviour
{
    //private Collider _collider;

    //private void Awake()
    //{
    //    _collider = GetComponent<Collider>();
    //}

    //private void OnTriggerEnter(Collider collider)
    //{
    //    if (collider.TryGetComponent(out HeroInput hero))
    //    {
    //        if (hero.IsUsedAbility)
    //        {
    //            hero.HeroRoundController.RoundCounterIncrement();
    //            return;
    //        }

    //        //var damagedClient =
    //        //    NetworkServer.connections.Where(id => id.Key == hero.connectionToClient.connectionId).FirstOrDefault()
    //        //        .Value.identity.gameObject.GetComponent<HeroInput>();

    //        hero.HeroColorChanger.ChangeColorFromDamage();
    //    }
    //}

    //public void AllowTrigger()
    //{
    //    _collider.isTrigger = true;
    //}

    //public void DisallowTrigger()
    //{
    //    _collider.isTrigger = false;
    //}

    public void CollisionCheck()
    {
        RaycastHit hit;
        var startPos = new Vector3(transform.position.x, transform.position.y + 1.2f, transform.position.z);

        Debug.DrawRay(startPos, transform.forward * 3f, Color.red);
        if (Physics.Raycast(startPos, transform.forward * 3f, out hit, 4f, LayerMask.GetMask("Hero")))
        {
            var player = hit.transform.parent.gameObject.GetComponent<HeroInput>();
            if (player != null)
            {
                Debug.Log(player);

                return;
            }
        }
    }
}
