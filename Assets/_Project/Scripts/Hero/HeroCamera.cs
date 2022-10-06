using UnityEngine;

public class HeroCamera : MonoBehaviour
{
    private IInputService _inputService;
    private HeroCameraRotator _cameraRotator;

    public void Init(Transform targetTransform, IInputService inputService)
    {
        _inputService = inputService;

        _inputService.OnMouseAxisXEvent += SetMouseAxisX;
        _inputService.OnMouseAxisYEvent += SetMouseAxisY;

        _cameraRotator = new HeroCameraRotator(transform, targetTransform);
    }

    private void OnDestroy()
    {
        _inputService.OnMouseAxisXEvent -= SetMouseAxisX;
        _inputService.OnMouseAxisYEvent -= SetMouseAxisY;
    }

    private void SetMouseAxisX(float axisX)
    {
        if (_cameraRotator != null)
            _cameraRotator.RotateX(axisX);
    }

    private void SetMouseAxisY(float axisY)
    {
        if (_cameraRotator != null)
            _cameraRotator.RotateY(axisY);
    }
}
