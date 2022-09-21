using System;
using UnityEngine;

public class InputService : MonoBehaviour
{
    private Hero _hero;
    private CameraRotator _heroCamera;

    public static Action<Hero> OnSetHeroToInputEvent;

    private void OnEnable()
    {
        OnSetHeroToInputEvent += SetHero;
    }

    private void OnDisable()
    {
        OnSetHeroToInputEvent -= SetHero;
    }

    private void Update()
    {
        //if (_hero == null) return;

        

        
    }

    public void SetHero(Hero hero)
    {
        //_hero = hero;
        //_heroCamera = _hero.GetComponentInChildren<CameraRotator>();
    }
}