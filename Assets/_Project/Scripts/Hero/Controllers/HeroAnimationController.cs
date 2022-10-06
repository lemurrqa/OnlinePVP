using UnityEngine;

public enum HeroAnimationType
{
    Run,
    Idle,
    Blink
}

public class HeroAnimationController
{
    private Animator _animator;
    private IInputService _inputService;

    public void Init(Animator animator, IInputService inputService)
    {
        _animator = animator;
        _inputService = inputService;

        _inputService.OnLeftMouseButtonInputEvent += PlayAnimationBlink;
    }

    public void OnDestroy()
    {
        _inputService.OnLeftMouseButtonInputEvent -= PlayAnimationBlink;
    }

    //public void PlayAnimationByType(HeroAnimationType typeAnimation)
    //{
    //    switch (typeAnimation)
    //    {
    //        case HeroAnimationType.Run:
    //            _animator.SetTrigger("Run");
    //            break;
    //        case HeroAnimationType.Blink:
    //            _animator.SetTrigger("Blink");
    //            break;
    //        default:
    //            break;
    //    }
    //}

    private void PlayAnimationBlink()
    {
        _animator.SetTrigger("Blink");
    }
}
