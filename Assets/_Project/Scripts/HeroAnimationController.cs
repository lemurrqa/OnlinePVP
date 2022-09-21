using UnityEngine;

public enum HeroAnimationType
{
    Run,
    Idle,
    Block
}

public class HeroAnimationController
{
    private Animator _animator;

    public HeroAnimationController(Animator animator)
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
            case HeroAnimationType.Idle:
                _animator.Play("Idle");
                break;
            case HeroAnimationType.Block:
                _animator.Play("Block");
                break;
            default:
                break;
        }
    }
}
