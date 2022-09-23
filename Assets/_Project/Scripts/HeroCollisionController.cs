using Mirror;
using UnityEngine;

public class HeroCollisionController : NetworkBehaviour
{
    public bool CanFinded;

    public void CollisionCheck()
    {
        if (CanFinded)
            return;

        RaycastHit hit;
        var startPos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);

        if (Physics.Raycast(startPos, transform.forward * 1.5f, out hit, 3f, LayerMask.GetMask("Hero")))
        {
            var player = hit.transform.parent.gameObject.GetComponentInParent<HeroInput>();
            if (player != null)
            {
                CalculateFindedPlayer(player);
                return;
            }
        }
    }

    private void CalculateFindedPlayer(HeroInput player)
    {
        Debug.Log(player);
        GetComponent<HeroInput>().HeroRoundController.RoundCounterIncrement();
        player.HeroColorChanger.ChangeColorFromDamage();
        CanFinded = true;
    }
}
