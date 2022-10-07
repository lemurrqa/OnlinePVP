using Mirror;
using UnityEngine;

public class Hero : NetworkBehaviour
{
    [SerializeField] private HeroCamera _heroCameraTemplate;

    private HeroCamera _heroCamera;
    private IInputService _inputService;
    private Animator _animator;
    private HeroScore _scoreController;
    private HeroView _heroView;
    private HeroMovementController _movementController;
    private HeroCollisionController _collisionController;
    private HeroAnimationController _animationController;
    private HeroAbilityController _abilityController;
    private HeroMaterialChanger _colorChanger;
    private HeroStateMachine _stateMachine;

    public HeroView HeroView => _heroView;
    public HeroScore ScoreController => _scoreController;
    public HeroStateMachine StateMachine => _stateMachine;
    public HeroCamera HeroCamera => _heroCamera;
    public HeroMaterialChanger MaterialChanger => _colorChanger;

    //[SyncVar]
    //private int _score;

    //public int Score
    //{
    //    get => _score;
    //    set
    //    {
    //        _score = value;
    //    }
    //}


    //[SyncVar(hook = nameof(OnTakeDamageChanged))]
    //private bool _isTakeDamage;

    //public bool IsTakeDamage
    //{
    //    get => _isTakeDamage;
    //    set
    //    {
    //        _isTakeDamage = value;
    //    }
    //}

    //private void OnTakeDamageChanged(bool oldValue, bool newValue)
    //{
    //    if (newValue == true)
    //        _stateMachine.Enter<HeroStateDamagable>();
    //}

    private void Awake()
    {
        _inputService = Mediator.GetInputServiceFunc?.Invoke();

        _animator = GetComponent<Animator>();
        _heroView = GetComponent<HeroView>();
        _colorChanger = GetComponent<HeroMaterialChanger>();
        _collisionController = GetComponent<HeroCollisionController>();
        _scoreController = GetComponent<HeroScore>();
        _heroCamera = Camera.main.GetComponent<HeroCamera>();
        _movementController = GetComponent<HeroMovementController>();
        _stateMachine = GetComponent<HeroStateMachine>();

        _animationController = new HeroAnimationController();
        _abilityController = new HeroAbilityController();

        _scoreController.Init(this);
        _colorChanger.Init(this);
        _animationController.Init(_animator);
        _collisionController.Init(this);
        _movementController.Init(this, _inputService);
        _abilityController.Init(this, _inputService);

        _movementController.SetStartPosition();
    }

    private void Start()
    {
        _heroView.SetNickname("Player" + UnityEngine.Random.Range(100, 999));
    }

    public override void OnStartLocalPlayer()
    {
        _stateMachine.Init(_animationController, _colorChanger);
        _stateMachine.Enter<HeroStateIdle>();

        _heroCamera = Camera.main.GetComponent<HeroCamera>();
        _heroCamera.Init(this, _inputService);
    }

    //[Command]
    //public void CmdSetStateDamagable()
    //{
    //    _stateMachine.Enter<HeroStateDamagable>();
    //}

    //public void ScoreIncrement()
    //{
    //    ScoreController.ScoreIncrement();
    //}

    public void SetWinnedStatus()
    {
        var levelResultService = Mediator.GetLevelResultServiceFunc?.Invoke();
        levelResultService.Complete(_heroView.Nickname);
    }

    //[Command]
    //public void CmdScoreIncrement()
    //{
    //    ScoreController.ScoreIncrement();
    //}

    [Command(requiresAuthority = false)]
    public void CmdPlayerWinnedStatus()
    {
        SetWinnedStatus();
    }
}
