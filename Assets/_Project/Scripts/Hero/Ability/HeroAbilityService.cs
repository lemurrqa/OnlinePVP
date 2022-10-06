using System.Linq;
using UnityEngine;

public enum AbilityType
{
    Blink
}

public class HeroAbilityService : MonoBehaviour
{
    [SerializeField] private HeroAblilityData _abilityData;

    public HeroAbility GetAbilityByType(AbilityType type)
    {
        HeroAbility ability = null;

        var data = _abilityData.data.Where(abil => abil.type == type).FirstOrDefault();

        switch (type)
        {
            case AbilityType.Blink:
                ability = new HeroAbilityBlink();
                ability.SetData(data);
                break;
            default:
                break;
        }

        return ability;
    }
}
