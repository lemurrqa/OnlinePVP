using Mirror;
using System.Collections;
using UnityEngine;

public class HeroColorChanger : NetworkBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    [SerializeField] private Material _damagableMaterial;
    [SerializeField] private int _changingSeconds = 3;

    private Hero _hero;
    private Material _previousMaterial;

    [SyncVar(hook = nameof(OnMaterialChanged))]
    private bool _isMaterialChange;

    public void Init(Hero hero)
    {
        _hero = hero;
        _previousMaterial = _meshRenderer.sharedMaterial;
    }

    public void ChangeMaterial()
    {
        _hero.IsInvulnerable = true;
        _isMaterialChange = true;
        StartCoroutine(MaterialChangedRoutine());
    }

    private void SetDefaultMaterial()
    {
        _hero.IsInvulnerable = false;
        _isMaterialChange = false;
    }

    private IEnumerator MaterialChangedRoutine()
    {
        yield return new WaitForSeconds(_changingSeconds);
        SetDefaultMaterial();
    }

    private void OnMaterialChanged(bool oldMatIndex, bool newMatIndex)
    {
        if (_isMaterialChange)
            _meshRenderer.sharedMaterial = _damagableMaterial;
        else
            _meshRenderer.sharedMaterial = _previousMaterial;
    }
}