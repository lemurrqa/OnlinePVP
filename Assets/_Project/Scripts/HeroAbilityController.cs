using UnityEngine;

public enum AbilityType
{
    Blink
}

public class HeroAbilityController
{
    private Rigidbody _rigidbody;

    public HeroAbilityController(Rigidbody rigidbody)
    {
        _rigidbody = rigidbody;
    }

    public void StartAbility(AbilityType abilityType)
    {
        var ability = AbilityService.GetAbilityFunction?.Invoke(abilityType);
        StartAbility(ability);
    }

    private void StartAbility(Ability ability)
    {
        ability.ResetAbility();
        ability.Init(_rigidbody);
        ability.Run();
    }
}
