public class HeroAbilityController
{
    private HeroAbilityService _abilityService;
    private IInputService _inputService;
    private Hero _hero;

    public void Init(Hero hero, IInputService inputService)
    {
        _hero = hero;
        _inputService = inputService;
        _abilityService = Mediator.GetAbilityServiceFunc?.Invoke();

        _inputService.OnLeftMouseButtonInputEvent += StartBlinkAbility;
    }

    private void StartBlinkAbility()
    {
        if (!_hero.isLocalPlayer)
            return;

        if (_hero.StateMachine.CheckActiveState(HeroStateType.UsingAbility))
            return;

        var ability = _abilityService.GetAbilityByType(AbilityType.Blink);

        ability.Init(_hero);
        ability.ResetAbility();
        ability.StartAbility();

        _hero.StateMachine.Enter<HeroStateUsingAbility>();
    }
}
