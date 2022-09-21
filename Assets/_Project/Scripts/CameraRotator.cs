using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    private Transform _targetTransform;
    private float _angleY;
    private float _angleX;

    public void Init(Transform target)
    {
        _targetTransform = target;
        _angleY = transform.rotation.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Rotate()
    {
        if (_targetTransform == null)
            return;

        RotateAroundTarget();
    }

    private void RotateAroundTarget()
    {
        _angleX += Input.GetAxis("Mouse X");
        _angleY -= Input.GetAxis("Mouse Y");

        _angleY = Mathf.Clamp(_angleY, -50, 80);

        transform.eulerAngles = new Vector3(_angleY, _angleX);
        transform.position = _targetTransform.position - transform.forward * 5f;
    }
}
