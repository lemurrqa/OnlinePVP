using Mirror;
using UnityEngine;

public class HeroInput : NetworkBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private HeroCameraRotator _heroCameraTemplate;
    [Header("Position variables")]
    [SerializeField] private float _speedMovement = 4f;
    [SerializeField] private float _speedRotation = 5f;
    [SerializeField] private float _startPosY = 0.3f;

    private HeroRoundController _heroRoundController;
    private HeroCollisionController _heroCollisionController;
    private HeroAbilityController _heroAbilityController;
    private HeroAnimationController _heroAnimationController;
    private HeroColorChanger _heroColorChanger;
    private HeroCameraRotator _heroCameraRotator;
    private float _rotationSmoothRef;
    private bool _isUsedAbility;

    public HeroRoundController HeroRoundController => _heroRoundController;
    public HeroColorChanger HeroColorChanger => _heroColorChanger;
    public bool IsUsedAbility => _isUsedAbility;

    private void Awake()
    {
        _heroRoundController = GetComponent<HeroRoundController>();
        _heroCollisionController = GetComponent<HeroCollisionController>();
        _heroColorChanger = GetComponent<HeroColorChanger>();
    }

    public override void OnStartLocalPlayer()
    {
        _heroCameraRotator = Instantiate(_heroCameraTemplate);

        _heroAnimationController = new HeroAnimationController(_animator);
        _heroAbilityController = new HeroAbilityController(this);

        transform.position = new Vector3(transform.position.x, _startPosY, transform.position.z);
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        CameraRotate();

        if (_isUsedAbility)
        {
            _heroCollisionController.CollisionCheck();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _heroAbilityController.StartAbility(AbilityType.Blink);
            _heroAnimationController.PlayAnimationByType(HeroAnimationType.Block);
            return;
        }

        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Rotate(input);
        Move(input);

        PlayAnimationByInput();
    }

    public void SetUsedAbility(bool isUsedAbility)
    {
        _heroCollisionController.CanFinded = !isUsedAbility;
        _isUsedAbility = isUsedAbility;
    }

    private void CameraRotate()
    {
        _heroCameraRotator.Rotate(transform);
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

        var targetRotation = aTan + _heroCameraRotator.transform.eulerAngles.y;

        transform.eulerAngles =
            Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _rotationSmoothRef, speed) * Vector3.up;
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
