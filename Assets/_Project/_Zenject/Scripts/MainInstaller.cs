using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField] private Mediator _mediator;
    [SerializeField] private InputService _inputService;
    [SerializeField] private ScoreInfoViewService _scoreInfoViewService;
    [SerializeField] private LevelResultService _resultService;
    [SerializeField] private Restarter _levelRestarter;
    [SerializeField] private HeroAbilityService _abilityService;
    [SerializeField] private CompleteScreen _completeScreen;

    public override void InstallBindings()
    {
        BindInputService();
        BindScoreViewService();
        BindResultService();
        BindLevelRestarter();
        BindAbilityService();
        BindCompleteScreen();
        BindMediator();
    }

    private void BindMediator()
    {
        Container.Bind<Mediator>().FromInstance(_mediator).AsSingle();
    }

    private void BindInputService()
    {
        Container.Bind<IInputService>().FromInstance(_inputService).AsSingle();
    }

    private void BindScoreViewService()
    {
        Container.Bind<ScoreInfoViewService>().FromInstance(_scoreInfoViewService).AsSingle();
    }

    private void BindResultService()
    {
        Container.Bind<LevelResultService>().FromInstance(_resultService).AsSingle();
    }

    private void BindLevelRestarter()
    {
        Container.Bind<Restarter>().FromInstance(_levelRestarter).AsSingle();
    }

    private void BindAbilityService()
    {
        Container.Bind<HeroAbilityService>().FromInstance(_abilityService).AsSingle();
    }
    
    private void BindCompleteScreen()
    {
        Container.Bind<CompleteScreen>().FromInstance(_completeScreen).AsSingle();
    }
}
