using Mirror;
using System.Collections;
using UnityEngine;

public class HeroColorChanger : NetworkBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    [SerializeField] private Material _changedMaterial;
    [SerializeField] private int _changeTimeInSeconds = 3;

    private Hero _hero;
    private Material _previousMaterial;

    public void Init(Hero hero)
    {
        _hero = hero;
    }

    public void ChangeColorFromDamage()
    {
        StartCoroutine(ChangedColorRoutine());
    }

    private IEnumerator ChangedColorRoutine()
    {
        _previousMaterial = _meshRenderer.sharedMaterial;
        _meshRenderer.sharedMaterial = _changedMaterial;

        yield return new WaitForSeconds(_changeTimeInSeconds);

        _meshRenderer.sharedMaterial = _previousMaterial;
        _hero.SetInvulnerable(false);
    }
}