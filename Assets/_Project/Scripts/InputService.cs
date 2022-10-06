using System;
using UnityEngine;

public class InputService : MonoBehaviour, IInputService
{
    public event Action OnLeftMouseButtonInputEvent;
    public event Action<Vector2> OnKeyboardInputEvent;
    public event Action<float> OnMouseAxisXEvent;
    public event Action<float> OnMouseAxisYEvent;

    public void Update()
    {
        OnMouseAxisXEvent?.Invoke(Input.GetAxis("Mouse X"));
        OnMouseAxisYEvent?.Invoke(Input.GetAxis("Mouse Y"));

        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (input != Vector2.zero)
        {
            OnKeyboardInputEvent?.Invoke(input);
        }

        if (Input.GetMouseButtonDown(0))
        {
            OnLeftMouseButtonInputEvent?.Invoke();
        }
    }
}
