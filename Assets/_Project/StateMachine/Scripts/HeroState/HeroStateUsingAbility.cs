public class HeroStateUsingAbility : HeroState
{
    private HeroAnimationController _animationController;

    public HeroStateUsingAbility(StateMachine stateMachine, HeroAnimationController animationController) : base(stateMachine)
    {
        _animationController = animationController;
    }

    public override void Enter()
    {
        _stateMachine.ActiveStateType = HeroStateType.UsingAbility;
        _animationController.PlayAnimationByType(HeroAnimationType.Blink);
    }
}
