using Mirror;
using TMPro;
using UnityEngine;

public class ConnectRoomUI : MonoBehaviour
{
    [SerializeField] private Canvas _connectCanvas;
    [SerializeField] private TMP_InputField _ipInputField;

    private NetworkManager _networkManager;

    private void Awake()
    {
        _networkManager = GetComponent<NetworkManager>();
    }

    public void StartHost()
    {
        _connectCanvas.gameObject.SetActive(false);
        _networkManager.StartHost();
    }

    public void StartClient()
    {
        if (string.IsNullOrEmpty(_ipInputField.text))
            return;

        _networkManager.networkAddress = _ipInputField.text;
        _connectCanvas.gameObject.SetActive(false);

        _networkManager.StartClient();
    }
}
