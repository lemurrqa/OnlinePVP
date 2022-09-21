using System.Collections;
using UnityEngine;

public class AbilityBlink : Ability
{
    private float _startTime;
    private float _time;
    private float _timeEnd = 0.7f;

    public override void ResetAbility()
    {
        _startTime = 0;
        _time = 0;
    }

    public override void Run()
    {
        StartCoroutine(RunRoutine());
    }

    private IEnumerator RunRoutine()
    {
        while (_time < _timeEnd)
        {
            yield return new WaitForSeconds(0.01f);

            var x = _rigidbody.transform.forward.x * _data.distance * _time;
            var y = _rigidbody.transform.forward.z * _data.distance * _time;

            _rigidbody.velocity = new Vector3(x, 0f, y);

            _time += 0.02f;

            if (_time >= _timeEnd)
            {
                _startTime = 0f;
                _rigidbody.velocity = Vector3.zero;
                break;
            }
        }
    }

    //public async void Run()
    //{
    //    Vector3 newPos;
    //    while (_time < 0.5f)
    //    {
    //        await RunAsync();

    //        newPos = new Vector3(_rigidbody.transform.forward.x * _distance * _time, 0f, _rigidbody.transform.forward.z * _distance * _time);
    //        _rigidbody.velocity = newPos;
    //        _time += 0.01f;

    //        if (_time >= 0.5f)
    //        {
    //            _startTime = 0f;
    //            _rigidbody.velocity = Vector3.zero;
    //            break;
    //        }
    //    }
    //}

    //private async Task RunAsync()
    //{
    //    await Task.Delay(TimeSpan.FromSeconds(0.01));
    //}
}
