using UnityEngine;

public class HeroCamera : MonoBehaviour
{
    private Hero _hero;
    private IInputService _inputService;
    private HeroCameraRotator _cameraRotator;

    public void Init(Hero hero, IInputService inputService)
    {
        _hero = hero;
        _inputService = inputService;

        _inputService.OnMouseAxisXEvent += SetMouseAxisX;
        _inputService.OnMouseAxisYEvent += SetMouseAxisY;

        _cameraRotator = new HeroCameraRotator(transform, _hero.transform);
    }

    private void SetMouseAxisX(float axisX)
    {
        if (!_hero.isLocalPlayer)
            return;

        _cameraRotator?.RotateX(axisX);
    }

    private void SetMouseAxisY(float axisY)
    {
        if (!_hero.isLocalPlayer)
            return;

        _cameraRotator?.RotateY(axisY);
    }
}
