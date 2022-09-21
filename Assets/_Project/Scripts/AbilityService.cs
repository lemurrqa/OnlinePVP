using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityService : MonoBehaviour
{
    [SerializeField] private HeroAblilityData _abilityData;

    private List<Ability> _abilities = new List<Ability>();

    public static Func<AbilityType, Ability> GetAbilityFunction;

    private void Awake()
    {
        _abilities = GetComponentsInChildren<Ability>().ToList();

        for (int i = 0; i < _abilities.Count; i++)
        {
            var data = _abilityData.data.Where(abil => abil.type == _abilities[i].Type).FirstOrDefault();

            if (data != null)
                _abilities[i].SetData(data);
        }

        GetAbilityFunction += GetAbilityByType;
    }

    public Ability GetAbilityByType(AbilityType type)
    {
        var ability = _abilities.Where(abil => abil.Data.type == type).FirstOrDefault();

        if (ability != null)
            return ability;

        return null;
    }
}
