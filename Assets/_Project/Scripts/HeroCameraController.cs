using UnityEngine;

public class HeroCameraController : MonoBehaviour
{
    [SerializeField] private Transform _heroTransform;
    [SerializeField] private CameraRotator _cameraRotator;

    private void Awake()
    {
        _cameraRotator = GetComponentInChildren<CameraRotator>();
    }

    private void Update()
    {
        _cameraRotator.SetTransformPoint(_heroTransform);
    }
}