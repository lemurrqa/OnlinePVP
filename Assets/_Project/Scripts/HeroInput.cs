using Mirror;
using UnityEngine;

public class HeroInput : NetworkBehaviour
{
    [SerializeField] private CameraRotator _cameraRotatorTemplate;
    [SerializeField] private HeroAbilityController _abilityController;
    [SerializeField] private Transform _targetForCamera;
    [SerializeField] private float _speedMovement = 4f;
    [SerializeField] private float _speedRotation = 5f;
    [SerializeField] private float _startPosY = 0.3f;

    private CameraRotator _cameraRotation;
    private HeroAnimationController _heroAnimationController;
    private IHeroMovement _heroMovement;
    private Animator _animator;
    private float _rotationSmoothRef;
    private bool _isRunningAbility;

    public override void OnStartLocalPlayer()
    {
        _cameraRotation = Instantiate(_cameraRotatorTemplate);
        _animator = GetComponent<Animator>();
        _heroMovement = new HeroMovementDefault();
        _heroAnimationController = new HeroAnimationController(_animator);
        _abilityController = new HeroAbilityController(this);

        transform.position = new Vector3(transform.position.x, _startPosY, transform.position.z);
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        CameraRotate();

        if (_isRunningAbility)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _abilityController.StartAbility(AbilityType.Blink);
            _heroAnimationController.PlayAnimationByType(HeroAnimationType.Block);
            return;
        }

        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Rotate(input);
        Move(input);

        PlayAnimationByInput();
    }

    public void SetRunningAbility(bool isRunning)
    {
        _isRunningAbility = isRunning;
    }

    private void CameraRotate()
    {
        _cameraRotation.Rotate(transform);
    }

    private void Move(Vector2 input)
    {
        var newPosition = transform.forward * (_speedMovement * input.normalized.sqrMagnitude) * Time.deltaTime;
        transform.Translate(newPosition, Space.World);
    }

    private void Rotate(Vector2 input)
    {
        if (input.normalized == Vector2.zero)
            return;

        var speed = Time.deltaTime * _speedRotation;
        var aTan = Mathf.Atan2(input.normalized.x, input.normalized.y) * Mathf.Rad2Deg;

        var targetRotation = aTan + _cameraRotation.transform.eulerAngles.y;

        transform.eulerAngles = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _rotationSmoothRef, speed) * Vector3.up;
    }

    private void PlayAnimationByInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            _heroAnimationController.PlayAnimationByType(HeroAnimationType.Run);
        }
        else
        {
            _heroAnimationController.PlayAnimationByType(HeroAnimationType.Idle);
        }
    }
}
