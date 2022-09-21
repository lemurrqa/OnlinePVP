using UnityEngine;

public class HeroMovementDefault : IHeroMovement
{
    public void Move(Transform transform, Vector3 newPos)
    {
        transform.Translate(newPos, Space.World);
    }
}