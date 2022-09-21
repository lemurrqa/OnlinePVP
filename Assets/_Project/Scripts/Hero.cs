using Mirror;
using UnityEngine;

public class Hero : NetworkBehaviour
{
    [SerializeField] private CameraRotator _cameraRotatorTemplate;
    [SerializeField] private Transform _targetForCamera;
    [SerializeField] private float _speedMovement = 4f;
    [SerializeField] private float _startPosY = 0.4f;

    private CameraRotator _cameraRotation;
    private HeroAnimationController _heroAnimationController;
    private IHeroMovement _heroMovement;
    private Animator _animator;
    private float _rotationSmoothRef;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _heroMovement = new HeroMovement();
        _heroAnimationController = new HeroAnimationController(_animator);

        _cameraRotation = Instantiate(_cameraRotatorTemplate);
        _cameraRotation.Init(_targetForCamera);

        transform.position = new Vector3(transform.position.x, _startPosY, transform.position.z);
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Rotate(input);
        Move(input);

        _cameraRotation.Rotate();

        PlayAnimationByInput();
    }

    private void Move(Vector2 input)
    {
        var newPosition = transform.forward * (_speedMovement * input.normalized.sqrMagnitude) * Time.deltaTime;
        _heroMovement.Move(transform, newPosition);
    }

    private void Rotate(Vector2 input)
    {
        if (input.normalized == Vector2.zero)
            return;

        var speed = Time.deltaTime * 5f;
        var aTan = Mathf.Atan2(input.normalized.x, input.normalized.y) * Mathf.Rad2Deg;

        var targetRotation = aTan + _cameraRotation.transform.eulerAngles.y;

        transform.eulerAngles = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _rotationSmoothRef, speed) * Vector3.up;
    }

    private void PlayAnimationByInput()
    {
        bool isInput = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (isInput)
        {
            _heroAnimationController.PlayAnimationByType(HeroAnimationType.Run);
        }
        else
        {
            _heroAnimationController.PlayAnimationByType(HeroAnimationType.Idle);
        }
    }
}
