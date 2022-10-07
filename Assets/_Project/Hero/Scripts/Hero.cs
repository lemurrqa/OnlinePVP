using Mirror;
using UnityEngine;

public class Hero : NetworkBehaviour
{
    private HeroCamera _heroCamera;
    private IInputService _inputService;
    private Animator _animator;
    private HeroScore _heroScore;
    private HeroView _heroView;
    private HeroStateMachine _stateMachine;
    private HeroMovementController _movementController;
    private HeroCollisionController _collisionController;
    private HeroAnimationController _animationController;
    private HeroAbilityController _abilityController;
    private HeroMaterialChanger _materialChanger;

    public HeroStateMachine StateMachine => _stateMachine;
    public HeroView HeroView => _heroView;
    public HeroScore HeroScore => _heroScore;
    public HeroCamera HeroCamera => _heroCamera;
    public HeroMaterialChanger MaterialChanger => _materialChanger;

    private void Awake()
    {
        _inputService = Mediator.GetInputServiceFunc?.Invoke();

        _animator = GetComponent<Animator>();
        _heroView = GetComponent<HeroView>();
        _materialChanger = GetComponent<HeroMaterialChanger>();
        _collisionController = GetComponent<HeroCollisionController>();
        _heroScore = GetComponent<HeroScore>();
        _heroCamera = Camera.main.GetComponent<HeroCamera>();
        _movementController = GetComponent<HeroMovementController>();
        _stateMachine = GetComponent<HeroStateMachine>();

        _animationController = new HeroAnimationController();
        _abilityController = new HeroAbilityController();

        _heroScore.Init(this);
        _materialChanger.Init(this);
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
        _stateMachine.Init(_animationController, _materialChanger);
        _stateMachine.Enter<HeroStateIdle>();

        _heroCamera = Camera.main.GetComponent<HeroCamera>();
        _heroCamera.Init(this, _inputService);
    }

    public void SetWinnedStatus()
    {
        var levelResultService = Mediator.GetLevelResultServiceFunc?.Invoke();
        levelResultService.Complete(_heroView.Nickname);
    }

    [Command(requiresAuthority = false)]
    public void CmdPlayerWinnedStatus()
    {
        SetWinnedStatus();
    }
}
