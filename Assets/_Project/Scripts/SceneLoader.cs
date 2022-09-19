using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _mainSceneName;

    private void Awake()
    {
        SceneManager.LoadScene(_mainSceneName);
    }
}
