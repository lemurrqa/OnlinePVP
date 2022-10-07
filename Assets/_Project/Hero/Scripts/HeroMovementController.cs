using Mirror;
using UnityEngine;

public class HeroMovementController : NetworkBehaviour
{
    [Header("Position variables")]
    [SerializeField] private float _speedMovement = 4f;
    [SerializeField] private float _speedRotation = 5f;
    [SerializeField] private float _startPosY = 0.3f;

    private Hero _hero;
    private IInputService _inputService;

    private float _rotationSmoothRef;

    public void Init(Hero hero, IInputService inputService)
    {
        _hero = hero;
        _inputService = inputService;

        _inputService.OnKeyboardInputEvent += OnKeyboardInput;
    }

    public void SetStartPosition()
    {
        transform.position = new Vector3(transform.position.x, _startPosY, transform.position.z);
    }

    public void OnKeyboardInput(Vector2 input)
    {
        if (!isLocalPlayer)
            return;

        if (_hero.StateMachine.CheckActiveState(HeroStateType.UsingAbility))
            return;

        Rotate(input);
        Move(input);

        if (input != Vector2.zero)
            _hero.StateMachine.Enter<HeroStateRun>();
        else
            _hero.StateMachine.Enter<HeroStateIdle>();
    }

    public void Move(Vector2 input)
    {
        var newPosition = transform.forward * (_speedMovement * input.normalized.sqrMagnitude) * Time.deltaTime;
        transform.Translate(newPosition, Space.World);
    }

    public void Rotate(Vector2 input)
    {
        if (input.normalized == Vector2.zero)
            return;

        var speed = Time.deltaTime * _speedRotation;
        var aTan = Mathf.Atan2(input.normalized.x, input.normalized.y) * Mathf.Rad2Deg;

        var targetRotation = aTan + _hero.HeroCamera.transform.eulerAngles.y;

        transform.eulerAngles =
            Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _rotationSmoothRef, speed) * Vector3.up;
    }
}
