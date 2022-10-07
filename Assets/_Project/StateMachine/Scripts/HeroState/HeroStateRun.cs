public class HeroStateRun : HeroState
{
    private HeroAnimationController _animationController;

    public HeroStateRun(StateMachine stateMachine, HeroAnimationController animationController) : base(stateMachine)
    {
        _animationController = animationController;
    }

    public override void Enter()
    {
        _stateMachine.ActiveStateType = HeroStateType.Run;
        _animationController.PlayAnimationByType(HeroAnimationType.Run);
    }
}
