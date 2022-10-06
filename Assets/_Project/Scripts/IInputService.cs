using System;
using UnityEngine;

public interface IInputService
{
    public event Action OnLeftMouseButtonInputEvent;
    public event Action<Vector2> OnKeyboardInputEvent;
    public event Action<float> OnMouseAxisXEvent;
    public event Action<float> OnMouseAxisYEvent;
}
