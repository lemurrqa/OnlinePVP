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
    private SceneUIController _sceneUIController;
    private PlayerInfoUI _heroInfoUI;
    private float _rotationSmoothRef;

    public HeroScoreController ScoreController => _scoreController;
    public HeroColorChanger ColorChanger => _colorChanger;
    public SceneUIController SceneUIController => _sceneUIController;

    [SyncVar(hook = nameof(OnNicknameChanged))]
    public string Nickname;

    [SyncVar]
    public bool IsUsedAbility;

    [SyncVar]
    public bool IsWin;

    [SyncVar]
    public bool IsInvulnerable;

    public Action OnColorChangedEvent;
    public Action<bool> OnUsingAbilityEvent;
    public Action<int> OnScoreChangedEvent;
    public Action<string> OnNicknameChangedEvent;

    private void Awake()
    {
        _sceneUIController = FindObjectOfType<SceneUIController>(); //sorry,it wont happen again
        _colorChanger = GetComponent<HeroColorChanger>();
        _collisionController = GetComponent<HeroCollisionController>();
        _scoreController = GetComponent<HeroScoreController>();
    }

    public override void OnStartClient()
    {
        _heroInfoUI = SceneUIController.SpawnAndGetInfoPanel();

        _colorChanger.Init(this);
        _collisionController.Init(this);
        _scoreController.Init(this);

        _animationController = new HeroAnimationController(_animator);
        _abilityController = new HeroAbilityController(this);

        OnColorChangedEvent = ColorChanger.ChangeColor;
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

        _heroInfoUI.SetLocalPlayer();
    }

    public override void OnStopClient()
    {
        ScoreController.ResetScore();

        OnColorChangedEvent = null;
        OnUsingAbilityEvent = null;
        OnNicknameChangedEvent = null;
        OnScoreChangedEvent = null;

        if (_heroInfoUI != null)
            _heroInfoUI.Hide();
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (IsWin)
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

    [Command]
    public void CmdScoreIncrement()
    {
        ScoreController.ScoreIncrement();
    }

    public void SetUseAbility(bool canUseAbility)
    {
        IsUsedAbility = canUseAbility;
    }

    public void SetWinnedStatus(bool isWin)
    {
        IsWin = isWin;
        SceneUIController.CompleteRound(Nickname);
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

    [Command]
    public void CmdSetName(string name)
    {
        Nickname = name;
    }

    private void OnNicknameChanged(string oldName, string newName)
    {
        OnNicknameChangedEvent?.Invoke(Nickname);
    }
}
