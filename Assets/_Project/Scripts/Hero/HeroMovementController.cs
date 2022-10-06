using UnityEngine;

public class HeroMovementController
{
    [Header("Position variables")]
    [SerializeField] private float _speedMovement = 4f;
    [SerializeField] private float _speedRotation = 5f;
    [SerializeField] private float _startPosY = 0.3f;

    private HeroCamera _heroCamera;
    private Hero _hero;
    private IInputService _inputService;

    private float _rotationSmoothRef;

    public void Init(Hero hero, HeroCamera heroCamera, IInputService inputService)
    {
        _hero = hero;
        _heroCamera = heroCamera;
        _inputService = inputService;

        _inputService.OnLeftMouseButtonInputEvent += OnMouseLeftButtonDown;
        _inputService.OnKeyboardInputEvent += OnKeyboardInput;
    }

    public void OnDestroy()
    {
        _inputService.OnLeftMouseButtonInputEvent -= OnMouseLeftButtonDown;
        _inputService.OnKeyboardInputEvent -= OnKeyboardInput;
    }

    public void SetStartPosition()
    {
        _hero.transform.position = new Vector3(_hero.transform.position.x, _startPosY, _hero.transform.position.z);
    }

    public void OnKeyboardInput(Vector2 input)
    {
        //if (!isLocalPlayer)
        //    return;

        //if (IsUsedAbility)
        //return;

        Rotate(input);
        Move(input);

        //if (input != Vector2.zero)
        //PlayAnimationByType(HeroAnimationType.Run);
        //else
        //PlayAnimationByType(HeroAnimationType.Idle);
    }

    public void OnMouseLeftButtonDown()
    {
        //if (!isLocalPlayer)
        //    return;

        //_abilityStarter.StartAbility(AbilityType.Blink);
        //PlayAnimationByType(HeroAnimationType.Blink);
    }

    private void Move(Vector2 input)
    {
        var newPosition = _hero.transform.forward * (_speedMovement * input.normalized.sqrMagnitude) * Time.deltaTime;
        _hero.transform.Translate(newPosition, Space.World);
    }

    private void Rotate(Vector2 input)
    {
        if (input.normalized == Vector2.zero)
            return;

        var speed = Time.deltaTime * _speedRotation;
        var aTan = Mathf.Atan2(input.normalized.x, input.normalized.y) * Mathf.Rad2Deg;

        var targetRotation = aTan + _heroCamera.transform.eulerAngles.y;

        _hero.transform.eulerAngles =
            Mathf.SmoothDampAngle(_hero.transform.eulerAngles.y, targetRotation, ref _rotationSmoothRef, speed) * Vector3.up;
    }
}
