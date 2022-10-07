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
    private bool _isMaterialChange;

    public bool IsMaterialChange => _isMaterialChange;

    public void Init(Hero hero)
    {
        _previousMaterial = _meshRenderer.sharedMaterial;
    }

    [Command(requiresAuthority = false)]
    public void CmdStartChange()
    {
        _isMaterialChange = true;
        StartCoroutine(MaterialChangedRoutine());
    }

    public void StopChange()
    {
        _isMaterialChange = false;
    }

    private IEnumerator MaterialChangedRoutine()
    {
        yield return new WaitForSeconds(_changingSeconds);
        StopChange();
    }

    private void OnMaterialChanged(bool oldMatIndex, bool newMatIndex)
    {
        if (_isMaterialChange)
            _meshRenderer.sharedMaterial = _damagableMaterial;
        else
            _meshRenderer.sharedMaterial = _previousMaterial;
    }
}