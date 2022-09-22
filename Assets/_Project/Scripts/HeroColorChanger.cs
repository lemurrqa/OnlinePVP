using Mirror;
using UnityEngine;

public class HeroColorChanger : NetworkBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    [SerializeField] private Color _changedColor;
    [SerializeField] private int _changeTimeInSeconds = 3;

    //private Color _previousColor;

    public void ChangeColorFromDamage()
    {
        //StartCoroutine(ChangedColorRoutine());
    }

    //private IEnumerator ChangedColorRoutine()
    //{
    //    yield return new WaitForSeconds(_changeTimeInSeconds);
    //}
}