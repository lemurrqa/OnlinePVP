using UnityEngine;
using Mirror;

public enum HeroAnimationType
{
    Run,
    Idle,
    Blink
}

public class HeroAnimationController
{
    private Animator _animator;

    public void Init(Animator animator)
    {
        _animator = animator;
    }

    public void PlayAnimationByType(HeroAnimationType typeAnimation)
    {
        switch (typeAnimation)
        {
            case HeroAnimationType.Run:
                _animator.Play("Run");
                break;
            case HeroAnimationType.Blink:
                _animator.Play("Blink");
                break;
            case HeroAnimationType.Idle:
                _animator.Play("Idle");
                break;
            default:
                break;
        }
    }
}
