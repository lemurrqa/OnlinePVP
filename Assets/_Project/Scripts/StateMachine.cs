using System;
using System.Collections.Generic;

public class StateMachine
{
    public IState _activeState;
    public Dictionary<Type, IState> _statesMap;

    public StateMachine()
    {
        _statesMap = new Dictionary<Type, IState>();
    }

    public void Enter<T>() where T : IState
    {
        _activeState?.Exit();

        var state = _statesMap[typeof(T)];
        _activeState = state;

        state.Enter();
    }

    public void Exit<T>() where T : IState
    {
        var state = _statesMap[typeof(T)];
        state.Exit();
    }
}

public class HeroState : IState
{
    public void Enter()
    {

    }

    public void Exit()
    {

    }
}

public interface IState
{
    public void Enter();
    public void Exit();
}