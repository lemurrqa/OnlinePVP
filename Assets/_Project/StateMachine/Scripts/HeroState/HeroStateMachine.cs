using System;
using System.Collections.Generic;

public class HeroStateMachine : StateMachine
{
    public void Init(HeroAnimationController animationController, HeroMaterialChanger heroColorChanger)
    {
        _statesMap = new Dictionary<Type, IState>();

        var stateIdle = new HeroStateIdle(this, animationController);
        var stateRun = new HeroStateRun(this, animationController);
        var stateUsingAbility = new HeroStateUsingAbility(this, animationController);

        AddState<HeroStateIdle>(stateIdle);
        AddState<HeroStateRun>(stateRun);
        AddState<HeroStateUsingAbility>(stateUsingAbility);
    }
}