public abstract class HeroState : IState
{
    protected StateMachine _stateMachine;

    public HeroState(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public abstract void Enter();

    public virtual void Exit()
    {

    }
}