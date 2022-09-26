using UnityEngine;

public class HeroCameraRotator : MonoBehaviour
{
    [SerializeField] private float _alongPlayerPosY;

    private float _angleY;
    private float _angleX;

    private void Start()
    {
        _angleY = transform.rotation.y;
    }

    public void Rotate(Transform target)
    {
        _angleX += Input.GetAxis("Mouse X");
        _angleY -= Input.GetAxis("Mouse Y");

        _angleY = Mathf.Clamp(_angleY, 0, 80);

        transform.eulerAngles = new Vector3(_angleY, _angleX);
        var alongTargetPos = new Vector3(target.position.x, _alongPlayerPosY, target.position.z);

        transform.position = alongTargetPos - transform.forward * 5f;
    }
}
