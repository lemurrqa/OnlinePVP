using UnityEngine;

public class HeroCameraRotator
{
    private Transform _cameraTransform;
    private Transform _targetTransform;
    private float _alongTargetPosY = 2.5f;
    private float _alongTargetPosZ = 5f;
    private float _angleY;
    private float _angleX;

    public HeroCameraRotator(Transform cameraTransform, Transform target)
    {
        _cameraTransform = cameraTransform;
        _targetTransform = target;

        _angleY = _cameraTransform.rotation.y;
    }

    public void RotateX(float axisX)
    {
        _angleX += axisX;

        _cameraTransform.eulerAngles = new Vector3(_cameraTransform.eulerAngles.x, _angleX);

        var alongTargetPos = new Vector3(_targetTransform.position.x, _alongTargetPosY, _targetTransform.position.z);

        _cameraTransform.position = alongTargetPos - _cameraTransform.forward * _alongTargetPosZ;
    }

    public void RotateY(float axisY)
    {
        _angleY -= axisY;

        _angleY = Mathf.Clamp(_angleY, 0, 80);

        _cameraTransform.eulerAngles = new Vector3(_angleY, _cameraTransform.eulerAngles.y);

        var alongTargetPos = new Vector3(_targetTransform.position.x, _alongTargetPosY, _targetTransform.position.z);

        _cameraTransform.position = alongTargetPos - _cameraTransform.forward * _alongTargetPosZ;
    }
}
