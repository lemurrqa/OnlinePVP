using Mirror;
using System.Collections;
using UnityEngine;

public class HeroMaterialChanger : NetworkBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    [SerializeField] private Material _damagableMaterial;
    [SerializeField] private int _changingSeconds = 3;

    private Material _previousMaterial;

    [SyncVar(hook = nameof(OnMaterialChanged))]
    private bool _canMaterialChange;

    public bool CanMaterialChange => _canMaterialChange;

    public void Init(Hero hero)
    {
        _previousMaterial = _meshRenderer.sharedMaterial;
    }

    [Command(requiresAuthority = false)]
    public void CmdStartChange()
    {
        _canMaterialChange = true;
        StartCoroutine(MaterialChangedRoutine());
    }

    public void StopChange()
    {
        _canMaterialChange = false;
    }

    private IEnumerator MaterialChangedRoutine()
    {
        yield return new WaitForSeconds(_changingSeconds);
        StopChange();
    }

    private void OnMaterialChanged(bool oldMatIndex, bool newMatIndex)
    {
        if (_canMaterialChange)
            _meshRenderer.sharedMaterial = _damagableMaterial;
        else
            _meshRenderer.sharedMaterial = _previousMaterial;
    }
}