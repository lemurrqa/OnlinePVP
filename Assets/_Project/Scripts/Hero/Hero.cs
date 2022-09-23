using Mirror;
using UnityEngine;

public class Hero : NetworkBehaviour
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
    private HeroUIController _heroUIController;
    private HeroColorChanger _heroColorChanger;
    private HeroCameraRotator _heroCameraRotator;
    private float _rotationSmoothRef;
    private bool _isUsedAbility;
    private bool _isInvulnerable;

    public HeroRoundController HeroRoundController => _heroRoundController;
    public HeroUIController HeroUIController => _heroUIController;
    public HeroColorChanger HeroColorChanger => _heroColorChanger;
    public bool IsUsedAbility => _isUsedAbility;
    public bool IsInvulnerable => _isInvulnerable;


    private void Awake()
    {
        _heroUIController = GetComponent<HeroUIController>();
        _heroColorChanger = GetComponent<HeroColorChanger>();

        _heroUIController.Init(this);
        _heroColorChanger.Init(this);
    }

    public override void OnStartLocalPlayer()
    {
        _heroCameraRotator = Instantiate(_heroCameraTemplate);

        _heroRoundController = new HeroRoundController(this);
        _heroCollisionController = new HeroCollisionController(this);
        _heroAbilityController = new HeroAbilityController(this);
        _heroAnimationController = new HeroAnimationController(_animator);

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
        PlayAnimationByInput(input);
    }

    public void SetInvulnerable(bool isInvulnerable)
    {
        _isInvulnerable = isInvulnerable;
    }

    public void SetUsedAbility(bool isUsedAbility)
    {
        _heroCollisionController.CmdSetCheckCollision(isUsedAbility);

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

    private void PlayAnimationByInput(Vector2 input)
    {
        if (input != Vector2.zero)
            _heroAnimationController.PlayAnimationByType(HeroAnimationType.Run);
        else
            _heroAnimationController.PlayAnimationByType(HeroAnimationType.Idle);
    }
}
