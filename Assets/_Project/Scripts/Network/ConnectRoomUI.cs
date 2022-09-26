using Mirror;
using TMPro;
using UnityEngine;

public class ConnectRoomUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _ipInputField;
    [SerializeField] private Canvas _connectCanvas;

    private NetworkManager _manager;

    private void Awake()
    {
        _manager = GetComponent<NetworkManager>();
    }

    public void StartHost()
    {
        _connectCanvas.gameObject.SetActive(false);
        _manager.StartHost();
    }

    public void StartClient()
    {
        if (string.IsNullOrEmpty(_ipInputField.text))
            return;

        _manager.networkAddress = _ipInputField.text;
        _connectCanvas.gameObject.SetActive(false);

        _manager.StartClient();
    }
}
