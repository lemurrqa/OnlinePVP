public class HeroStateIdle : HeroState
{
    private HeroAnimationController _animationController;

    public HeroStateIdle(StateMachine stateMachine, HeroAnimationController animationController)
        : base(stateMachine)
    {
        _animationController = animationController;
    }

    public override void Enter()
    {
        _stateMachine.ActiveStateType = HeroStateType.Idle;
        _animationController.PlayAnimationByType(HeroAnimationType.Idle);
    }
}