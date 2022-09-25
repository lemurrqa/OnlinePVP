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

    public void ChangeColor()
    {
        _hero.IsInvulnerable = true;

        _previousMaterial = _meshRenderer.sharedMaterial;
        _meshRenderer.sharedMaterial = _changedMaterial;

        StartCoroutine(ChangedColorRoutine());
    }

    [Command]
    public void CmdChangeColor()
    {
        _hero.IsInvulnerable = true;

        _previousMaterial = _meshRenderer.sharedMaterial;
        _meshRenderer.sharedMaterial = _changedMaterial;

        StartCoroutine(ChangedColorRoutine());
    }

    private void SetColorDefault()
    {
        _meshRenderer.sharedMaterial = _previousMaterial;
        _hero.IsInvulnerable = false;
    }

    private IEnumerator ChangedColorRoutine()
    {
        yield return new WaitForSeconds(_changeTimeInSeconds);

        SetColorDefault();
    }
}