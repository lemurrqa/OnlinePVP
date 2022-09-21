using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [SerializeField] protected AbilityType _type;

    protected HeroAblilityData.Data _data;
    protected Rigidbody _rigidbody;

    public AbilityType Type => _type;
    public HeroAblilityData.Data Data => _data;

    public void SetData(HeroAblilityData.Data data)
    {
        _data = data;
    }

    public virtual void Init(Rigidbody rigidbody)
    {
        _rigidbody = rigidbody;
    }

    public virtual void ResetAbility()
    {

    }

    public virtual void Run()
    {

    }
}