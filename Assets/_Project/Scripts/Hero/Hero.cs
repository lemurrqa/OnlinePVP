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
    private HeroUIController _UIController;
    private HeroColorChanger _colorChanger;
    private HeroCameraRotator _heroCameraRotator;
    private SceneUIController _sceneUIController;
    private PlayerInfoUI _heroInfoUI;
    private float _rotationSmoothRef;

    public HeroScoreController ScoreController => _scoreController;
    public HeroUIController UIController => _UIController;
    public HeroColorChanger ColorChanger => _colorChanger;
    public SceneUIController SceneUIController => _sceneUIController;

    //[SyncVar(hook = nameof(OnChangedScore))]
    //public int score = 0;

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
        _UIController = GetComponent<HeroUIController>();
        _colorChanger = GetComponent<HeroColorChanger>();
        _collisionController = GetComponent<HeroCollisionController>();
        _scoreController = GetComponent<HeroScoreController>();
    }

    public override void OnStartClient()
    {
        _heroInfoUI = SceneUIController.SpawnAndGetInfoPanel();

        _UIController.Init(this);
        _colorChanger.Init(this);
        _collisionController.Init(this);
        _scoreController.Init(this);

        _animationController = new HeroAnimationController(_animator);
        _abilityController = new HeroAbilityController(this);

        OnColorChangedEvent = ColorChanger.ChangeColor;
        OnUsingAbilityEvent = SetUseAbility;
        OnNicknameChangedEvent = _heroInfoUI.OnPlayerNicknameChanged;
        OnScoreChangedEvent = _heroInfoUI.OnPlayerScoreChanged;
        
        _UIController.CmdSetName("Player" + UnityEngine.Random.Range(100, 999));

        OnNicknameChangedEvent?.Invoke(UIController.Nickname);
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
            _animationController.PlayAnimationByType(HeroAnimationType.Block);
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
        //_collisionController.CanCheckCollision(true);

        IsUsedAbility = canUseAbility;
    }

    public void SetWinnedStatus(bool isWin)
    {
        IsWin = isWin;
        SceneUIController.CompleteRound(UIController.Nickname);
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

        _UIController.SetRotationTextsFromCamera(_heroCameraRotator.transform.eulerAngles);
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

    //private void PlayAnimationByInput(Vector2 input)
    //{
    //    if (input != Vector2.zero)
    //    {
    //        _animationController.PlayAnimationByType(HeroAnimationType.Run);
    //        return;
    //    }

    //    _animationController.PlayAnimationByType(HeroAnimationType.Idle);
    //}

    private void PlayAnimationByType(HeroAnimationType typeAnimation)
    {
        _animationController.PlayAnimationByType(typeAnimation);
    }

    //private void OnChangedUsedAbility(bool old, bool newUsedAbility)
    //{
    //    _collisionController.CanCheckCollision(true);
    //}

    //private void OnChangedScore(int oldScore, int newScore)
    //{
    //    OnScoreChangedEvent?.Invoke(score);
    //}
}
