using System;
using UnityEngine;
using Zenject;

public class Mediator : MonoBehaviour
{
    [Inject] private IInputService _inputService;
    [Inject] private ScoreInfoViewService _scoreViewService;
    [Inject] private HeroAbilityService _abilityService;
    [Inject] private LevelResultService _levelResultService;
    [Inject] private Restarter _levelRestarter;
    [Inject] private CompleteScreen _completeScreen;

    public static Func<IInputService> GetInputServiceFunc;
    public static Func<ScoreInfoViewService> GetScoreViewServiceFunc;
    public static Func<HeroAbilityService> GetAbilityServiceFunc;
    public static Func<LevelResultService> GetLevelResultServiceFunc;

    public CompleteScreen CompleteScreen => _completeScreen;

    private void Awake()
    {
        GetScoreViewServiceFunc = GetScoreViewService;
        GetInputServiceFunc = GetInputService;
        GetAbilityServiceFunc = GetAbilityService;
        GetLevelResultServiceFunc = GetLevelResultService;
    }

    public void RestartLevel() => _levelRestarter.RestartLevel();

    private IInputService GetInputService() => _inputService;
    private ScoreInfoViewService GetScoreViewService() => _scoreViewService;
    private HeroAbilityService GetAbilityService() => _abilityService;
    private LevelResultService GetLevelResultService() => _levelResultService;
}
