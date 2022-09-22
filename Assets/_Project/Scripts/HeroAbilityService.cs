using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroAbilityService : MonoBehaviour
{
    [SerializeField] private HeroAblilityData _abilityData;

    private List<HeroAbility> _abilities = new List<HeroAbility>();

    public static Func<AbilityType, HeroAbility> GetAbilityFunction;

    private void Awake()
    {
        GetAbilityFunction += GetAbilityByType;
    }

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
