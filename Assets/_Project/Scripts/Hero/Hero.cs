using Mirror;
using System;
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

    public HeroView HeroView => _heroView;
    public HeroScore ScoreController => _scoreController;

    [SyncVar]
    public bool IsUsedAbility;

    [SyncVar]
    public bool IsInvulnerable;

    public Action<bool> OnUsingAbilityEvent;

    private void Awake()
    {
        _inputService = Mediator.GetInputServiceFunc?.Invoke();

        _animator = GetComponent<Animator>();
        _animationController = new HeroAnimationController();
        _movementController = new HeroMovementController();
        _abilityController = new HeroAbilityController();
        _collisionController = GetComponent<HeroCollisionController>();
        _heroView = GetComponent<HeroView>();
        _scoreController = GetComponent<HeroScore>();
    }

    private void OnDestroy()
    {
        _animationController.OnDestroy();
        _abilityController.OnDestroy();
        _movementController.OnDestroy();
    }

    public override void OnStartClient()
    {
        CmdSetName("Player" + UnityEngine.Random.Range(100, 999));

        _collisionController.Init(this);
        _scoreController.Init(this);

        OnUsingAbilityEvent = SetUseAbility;
        
    }

    public override void OnStartLocalPlayer()
    {
        _heroCamera = Instantiate(_heroCameraTemplate);

        _heroCamera.Init(transform, _inputService);
        _animationController.Init(_animator, _inputService);
        _movementController.Init(this, _heroCamera, _inputService);
        _abilityController.Init(this, _inputService);

        _movementController.SetStartPosition();
    }

    public override void OnStopClient()
    {
        _scoreController.ResetScore();

        OnUsingAbilityEvent = null;
    }

    public void ScoreIncrement()
    {
        ScoreController.ScoreIncrement();
    }

    public void ChangeColor()
    {
        IsInvulnerable = true;
        _heroView.ColorChanger.StartChange();
    }

    public void SetWinnedStatus()
    {
        var levelResultService = Mediator.GetLevelResultServiceFunc?.Invoke();
        levelResultService.Complete(_heroView.Nickname);
    }

    [Command]
    public void CmdScoreIncrement()
    {
        ScoreController.ScoreIncrement();
    }

    [Command(requiresAuthority = false)]
    public void CmdChangeColor()
    {
        ChangeColor();
    }

    [Command]
    public void CmdSetName(string name)
    {
        _heroView.SetNickname(name);
    }

    [Command]
    public void CmdPlayerWinnedStatus()
    {
        SetWinnedStatus();
    }

    private void SetUseAbility(bool canUseAbility)
    {
        IsUsedAbility = canUseAbility;
    }

    //private void PlayAnimationByType(HeroAnimationType typeAnimation)
    //{
    //    _animationController.PlayAnimationByType(typeAnimation);
    //}
}
