using UnityEngine;

public class HeroMovement : IHeroMovement
{
    public void Move(Transform transform, Vector3 newPos)
    {
        transform.Translate(newPos, Space.World);
    }
}