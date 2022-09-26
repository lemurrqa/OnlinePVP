using UnityEngine;

public class HeroAbility
{
    [SerializeField] protected AbilityType _type;

    protected HeroAblilityData.Data _data;
    protected Hero _hero;

    public void SetData(HeroAblilityData.Data data)
    {
        _data = data;
    }

    public virtual void Init(Hero hero)
    {
        _hero = hero;
    }

    public virtual void ResetAbility() { }

    public virtual void Run() { }
}