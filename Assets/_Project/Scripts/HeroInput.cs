using UnityEngine;

public class HeroInput : MonoBehaviour
{
    private IHeroMovement _heroMovement;
    private Animator _animator;
    private HeroAnimationController _heroAnimationController;
    private CameraRotator _cameraRotator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _cameraRotator = GetComponentInChildren<CameraRotator>();
    }

    private void Start()
    {
        Init();
        transform.position = new Vector3(transform.position.x, 0.4f, transform.position.z);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            _heroAnimationController.PlayAnimationByType(HeroAnimationType.Run);
        }
        else
        {
            _heroAnimationController.PlayAnimationByType(HeroAnimationType.Idle);
        }

        var newPos = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        _heroMovement.Move(transform, newPos * Time.deltaTime * 3f);
        _heroMovement.Rotate(transform, new Vector3(0f, _cameraRotator.transform.rotation.eulerAngles.y, 0f));
    }

    private void Init()
    {
        _heroMovement = new HeroMovement();
        _heroAnimationController = new HeroAnimationController(_animator);
    }
}