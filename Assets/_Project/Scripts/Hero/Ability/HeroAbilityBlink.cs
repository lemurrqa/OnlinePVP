using System;
using System.Threading.Tasks;
using UnityEngine;

public class HeroAbilityBlink : HeroAbility
{
    private float _startTime;
    private float _time;
    private float _timeEnd = 0.7f;

    public override void ResetAbility()
    {
        _startTime = 0;
        _time = 0;
    }

    public override async void Run()
    {
        _hero.OnUsingAbilityEvent?.Invoke(true);

        while (_time < _timeEnd)
        {
            await DelayAsync(0.005f);

            _hero.transform.position += GetNewPosition();

            _time += 0.014f;

            if (_time >= _timeEnd)
            {
                _startTime = 0f;
                _hero.transform.position += Vector3.zero;

                _hero.OnUsingAbilityEvent?.Invoke(false);

                break;
            }
        }
    }

    private async Task DelayAsync(float seconds)
    {
        await Task.Delay(TimeSpan.FromSeconds(seconds));
    }

    private Vector3 GetNewPosition()
    {
        var x = _hero.transform.forward.x * _data.distance * Time.fixedDeltaTime;
        var y = _hero.transform.forward.z * _data.distance * Time.fixedDeltaTime;

        return new Vector3(x, 0f, y);
    }
}
