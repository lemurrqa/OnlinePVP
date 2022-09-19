using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void FixedUpdate()
    {
        var axisX = Input.GetAxis("Mouse X");

        var rotationWithoutZ = Quaternion.Euler(9f, axisX + transform.localRotation.y, 0f);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, rotationWithoutZ, Time.fixedDeltaTime * 10f);
    }

    public void SetTransformPoint(Transform transform)
    {
        var inversePos = transform.InverseTransformPoint(transform.position);
        transform.position = transform.TransformPoint(inversePos);
        transform.LookAt(transform);
    }
}
