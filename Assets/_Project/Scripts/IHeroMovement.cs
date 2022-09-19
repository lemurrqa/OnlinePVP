using UnityEngine;

public interface IHeroMovement
{
    public void Move(Transform transform, Vector3 newPos);
    public void Rotate(Transform transform, Vector3 newPos);
}