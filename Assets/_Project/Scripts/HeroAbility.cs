using UnityEngine;

public class HeroAbility
{
    [SerializeField] protected AbilityType _type;

    protected HeroAblilityData.Data _data;
    protected HeroInput _hero;

    public void SetData(HeroAblilityData.Data data)
    {
        _data = data;
    }

    public virtual void Init(HeroInput hero)
    {
        _hero = hero;
    }

    public virtual void ResetAbility()
    {

    }

    public virtual void Run()
    {

    }
}