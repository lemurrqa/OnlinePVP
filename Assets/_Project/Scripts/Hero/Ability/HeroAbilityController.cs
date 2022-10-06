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

    public void OnDestroy()
    {
        _inputService.OnLeftMouseButtonInputEvent -= StartBlinkAbility;
    }

    //public void StartAbilityByType(AbilityType abilityType)
    //{
    //    var ability = _abilityService.GetAbilityByType(abilityType);

    //    ability.ResetAbility();
    //    ability.Init(_hero);
    //    ability.Run();
    //}

    private void StartBlinkAbility()
    {
        var ability = _abilityService.GetAbilityByType(AbilityType.Blink);

        ability.ResetAbility();
        ability.Init(_hero);
        ability.Run();
    }
}
