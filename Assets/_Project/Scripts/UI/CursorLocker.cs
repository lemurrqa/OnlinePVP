using UnityEngine;

public class CursorLocker : MonoBehaviour
{
    [SerializeField] private CursorLockMode _cursorLockMode;
    [SerializeField] private bool _isVisible;

    private void Start()
    {
        Cursor.lockState = _cursorLockMode;
        Cursor.visible = _isVisible;
    }
}
