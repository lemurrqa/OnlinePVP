public class HeroAbilityController
{
    private Hero _hero;

    public HeroAbilityController(Hero hero)
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
