public enum AbilityType
{
    Blink
}

public class HeroAbilityController
{
    private HeroInput _hero;

    public HeroAbilityController(HeroInput hero)
    {
        _hero = hero;
    }

    public void StartAbility(AbilityType abilityType)
    {
        var ability = HeroAbilityService.GetAbilityFunction?.Invoke(abilityType);

        ability.ResetAbility();
        ability.Init(_hero);
        ability.Run();
    }
}
