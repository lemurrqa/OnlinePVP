using Mirror;
using System;
using UnityEngine;

public enum HeroState
{
    Idle,
    Invulnerable,
    UsedAbility
}

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
    private HeroColorChanger _colorChanger;
    private HeroCameraRotator _heroCameraRotator;
    private PlayerInfoUI _heroInfoUI;
    private float _rotationSmoothRef;

    public HeroScoreController ScoreController => _scoreController;
    public HeroColorChanger ColorChanger => _colorChanger;

    [SyncVar(hook = nameof(OnNicknameChanged))]
    public string Nickname;

    [SyncVar]
    public bool IsUsedAbility;

    [SyncVar]
    public bool IsWin;

    [SyncVar]
    public bool IsInvulnerable;

    public Action<bool> OnUsingAbilityEvent;
    public Action<int> OnScoreChangedEvent;
    public Action<string> OnNicknameChangedEvent;

    private void Awake()
    {
        _colorChanger = GetComponent<HeroColorChanger>();
        _collisionController = GetComponent<HeroCollisionController>();
        _scoreController = GetComponent<HeroScoreController>();
    }

    public override void OnStartClient()
    {
        _heroInfoUI = SceneUIService.Instance.SpawnAndGetInfoPanel();
        _colorChanger.Init(this);
        _collisionController.Init(this);
        _scoreController.Init(this);

        _animationController = new HeroAnimationController(_animator);
        _abilityController = new HeroAbilityController(this);

        OnUsingAbilityEvent = SetUseAbility;
        OnNicknameChangedEvent = _heroInfoUI.OnPlayerNicknameChanged;
        OnScoreChangedEvent = _heroInfoUI.OnPlayerScoreChanged;

        CmdSetName("Player" + UnityEngine.Random.Range(100, 999));

        OnNicknameChangedEvent?.Invoke(Nickname);
        transform.position = new Vector3(transform.position.x, _startPosY, transform.position.z);
    }

    public override void OnStartLocalPlayer()
    {
        _heroCameraRotator = Instantiate(_heroCameraTemplate);
        if (_heroInfoUI != null)
            _heroInfoUI.SetLocalPlayer();
    }

    public override void OnStopClient()
    {
        ScoreController.ResetScore();

        OnUsingAbilityEvent = null;
        OnNicknameChangedEvent = null;
        OnScoreChangedEvent = null;

        if (_heroInfoUI != null)
        {
            Destroy(_heroInfoUI.gameObject);
            NetworkServer.Destroy(_heroInfoUI.gameObject);
        }
    }

    private void Update()
    {
        if (IsWin)
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

    private void SetUseAbility(bool canUseAbility)
    {
        IsUsedAbility = canUseAbility;
    }

    public void SetWinnedStatus(bool isWin)
    {
        IsWin = isWin;
        SceneUIService.Instance.CompleteRound(Nickname);
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
        Nickname = name;
    }

    [Command]
    public void CmdPlayerWinnedStatus(bool isWin)
    {
        SetWinnedStatus(isWin);
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

    private void OnNicknameChanged(string oldName, string newName)
    {
        OnNicknameChangedEvent?.Invoke(Nickname);
    }
}
