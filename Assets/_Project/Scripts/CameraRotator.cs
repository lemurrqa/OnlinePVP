using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    private Transform _targetTransform;
    private Vector3 _pos;
    private float _currentRotationY = 0f;
    private float _angleY;
    private float _angleX;
    public void Init(Transform target)
    {
        _targetTransform = target;
        _pos = _targetTransform.InverseTransformPoint(transform.position);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _angleY = transform.rotation.y;
    }

    public void Rotate()
    {
        if (_targetTransform == null)
            return;

        FixedCameraToTarget();
        RotationCameraAroundTarget();
    }

    private void FixedCameraToTarget()
    {
        //var oldRotation = _targetTransform.rotation;
        //_targetTransform.rotation = Quaternion.Euler(oldRotation.eulerAngles.x, 0f, oldRotation.eulerAngles.z);

        //var curPos = _targetTransform.TransformPoint(_pos);

        //_targetTransform.rotation = oldRotation;

        //transform.position = curPos;

        _angleX += Input.GetAxis("Mouse X");
        _angleY -= Input.GetAxis("Mouse Y");

        _angleY = Mathf.Clamp(_angleY, -50, 80);

        transform.eulerAngles = new Vector3(_angleY, _angleX);
        transform.position = _targetTransform.position - transform.forward * 5f;
    }

    private void RotationCameraAroundTarget()
    {
        //var axisX = Input.GetAxis("Mouse X");
        //var axisY = Input.GetAxis("Mouse Y");

        //var rot = _currentRotationY + axisX * 360f * Time.deltaTime;
        //transform.RotateAround(transform.position, _targetTransform.up, rot);
        //_currentRotationY = rot;
        //transform.LookAt(_targetTransform);
    }
}
