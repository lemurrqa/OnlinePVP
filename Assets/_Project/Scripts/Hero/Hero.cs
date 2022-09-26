using Mirror;
using System;
using UnityEngine;

public class Hero : NetworkBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private HeroCameraRotator _heroCameraTemplate;
    [Header("Position variables")]
    [SerializeField] private float _speedMovement = 4f;
    [SerializeField] private float _speedRotation = 5f;
    [SerializeField] private float _startPosY = 0.3f;

    private HeroScoreController _scoreController;
    private HeroCollisionController _collisionController;
    private HeroAbilityController _abilityController;
    private HeroAnimationController _animationController;
    private HeroUIController _heroUIController;
    private HeroColorChanger _colorChanger;
    private HeroCameraRotator _heroCameraRotator;
    private float _rotationSmoothRef;

    public HeroScoreController ScoreController => _scoreController;
    public HeroUIController HeroUIController => _heroUIController;
    public HeroColorChanger ColorChanger => _colorChanger;

    [SyncVar]
    public bool IsUsedAbility;

    [SyncVar]
    public bool IsInvulnerable;

    [SyncVar]
    public bool IsStoppedInput;

    public Action<bool> OnUsingAbilityEvent;

    private void Awake()
    {
        _colorChanger = GetComponent<HeroColorChanger>();
        _collisionController = GetComponent<HeroCollisionController>();
        _scoreController = GetComponent<HeroScoreController>();
        _heroUIController = GetComponent<HeroUIController>();

        _heroUIController.Init(this);
    }

    public override void OnStartClient()
    {
        CmdSetName("Player" + UnityEngine.Random.Range(100, 999));

        _heroUIController.StartClient();

        _colorChanger.Init(this);
        _collisionController.Init(this);
        _scoreController.Init(this);

        _animationController = new HeroAnimationController(_animator);
        _abilityController = new HeroAbilityController(this);

        OnUsingAbilityEvent = SetUseAbility;

        transform.position = new Vector3(transform.position.x, _startPosY, transform.position.z);
    }

    public override void OnStartLocalPlayer()
    {
        _heroUIController.StartLocalPlayer();
        _heroCameraRotator = Instantiate(_heroCameraTemplate);
    }

    public override void OnStopClient()
    {
        _heroUIController.StopClient();
        _scoreController.ResetScore();

        OnUsingAbilityEvent = null;
    }

    private void Update()
    {
        if (IsStoppedInput)
            return;

        if (!isLocalPlayer)
            return;

        CameraRotate();

        if (IsUsedAbility)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _abilityController.StartAbility(AbilityType.Blink);
            PlayAnimationByType(HeroAnimationType.Block);
            return;
        }

        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Rotate(input);
        Move(input);

        if (input != Vector2.zero)
            PlayAnimationByType(HeroAnimationType.Run);
        else
            PlayAnimationByType(HeroAnimationType.Idle);
    }

    public void ScoreIncrement()
    {
        ScoreController.ScoreIncrement();
    }

    public void ChangeColor()
    {
        ColorChanger.ChangeMaterial();
    }

    public void SetWinnedStatus()
    {
        RpcStopPlayerInput();
        SceneUIService.Instance.CompleteRound(_heroUIController.GetNickname());
    }

    [Command]
    public void CmdScoreIncrement()
    {
        ScoreController.ScoreIncrement();
    }

    [Command(requiresAuthority = false)]
    public void CmdChangeColor()
    {
        ColorChanger.ChangeMaterial();
    }

    [Command]
    public void CmdSetName(string name)
    {
        _heroUIController.SetNickname(name);
    }

    [Command]
    public void CmdPlayerWinnedStatus()
    {
        SetWinnedStatus();
    }

    [ClientRpc]
    private void RpcStopPlayerInput()
    {
        IsStoppedInput = true;
    }

    private void SetUseAbility(bool canUseAbility)
    {
        IsUsedAbility = canUseAbility;
    }

    private void CameraRotate()
    {
        if (_heroCameraRotator == null)
            return;

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

    private void PlayAnimationByType(HeroAnimationType typeAnimation)
    {
        _animationController.PlayAnimationByType(typeAnimation);
    }
}
