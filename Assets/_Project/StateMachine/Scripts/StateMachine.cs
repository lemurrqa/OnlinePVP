using Mirror;
using System;
using System.Collections.Generic;

public enum HeroStateType
{
    Idle,
    Run,
    UsingAbility,
}

public class StateMachine : NetworkBehaviour
{
    private IState _activeState;
    protected Dictionary<Type, IState> _statesMap;

    [SyncVar]
    protected HeroStateType _activeStateType;

    public HeroStateType ActiveStateType
    {
        get => _activeStateType;
        set
        {
            _activeStateType = value;
        }
    }

    public virtual void AddState<T>(IState state) where T : IState
    {
        if (!_statesMap.ContainsKey(typeof(T)))
        {
            _statesMap.Add(typeof(T), state);
        }
    }

    public virtual void Enter<T>() where T : IState
    {
        _activeState?.Exit();

        var state = _statesMap[typeof(T)];  
        _activeState = state;

        state.Enter();
    }

    public virtual void Exit<T>() where T : IState
    {
        var state = _statesMap[typeof(T)];
        state.Exit();
    }

    public bool CheckActiveState(HeroStateType stateType)
    {
        if (_activeStateType == stateType)
            return true;

        return false;
    }
}