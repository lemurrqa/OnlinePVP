using UnityEngine;

public class ScoreInfoViewService : MonoBehaviour
{
    [SerializeField] private ScoreInfoView _heroInfoTemplate;
    [SerializeField] private Transform _heroInfoParent;

    public ScoreInfoView SpawnAndGetInfoPanel()
    {
        var playerUIObject = Instantiate(_heroInfoTemplate, _heroInfoParent);
        return playerUIObject;
    }
}
