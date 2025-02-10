using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Joystick _leftJoystick, _rightJoystick;

    [SerializeField] private float _speed = 30f;
    [SerializeField] private float _rotationSpeed = 2f;
    [SerializeField] private float _tiltAngle = 30f;

    [SerializeField] private ParticleSystem[] _particles;

    private Rigidbody _rigidbody;
    private Vector3 _movementInput;
    private Vector3 _rotationInput;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();

#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
    }

    private void Update()
    {
#if UNITY_EDITOR
        _movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized
              + new Vector3(0, Input.GetAxis("Mouse Y"), 0);
        _rotationInput = new Vector3(0, Input.GetAxis("Mouse X"), 0);
#elif UNITY_ANDROID
        _movementInput = new Vector3(_leftJoystick.Direction.x, _rightJoystick.Direction.y, _leftJoystick.Direction.y);
        _rotationInput = new Vector3(0, _rightJoystick.Direction.x, 0);
#endif
    }

    private void FixedUpdate()
    {
        if (_movementInput != Vector3.zero)
        {
            _animator.SetBool("IsFlying", true);
            foreach(var particle in _particles)
            {
                if (!particle.isPlaying)
                    particle.Play();
            }

            Vector3 movementDirection = transform.forward * _movementInput.z + transform.right * _movementInput.x + transform.up * _movementInput.y;
            _rigidbody.velocity = movementDirection * _speed;

            Vector3 targetRotation = new Vector3(-_movementInput.y * _tiltAngle, transform.rotation.eulerAngles.y, -_movementInput.x * _tiltAngle);

            transform.rotation = Quaternion.Lerp(transform.rotation, 
                Quaternion.Euler(targetRotation), 
                _rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            _animator.SetBool("IsFlying", false);
            foreach (var particle in _particles)
            {
                if (particle.isPlaying)
                    particle.Stop();
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, 
                Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0)), 
                _rotationSpeed * Time.fixedDeltaTime);

            _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, Vector3.zero, _rotationSpeed * Time.fixedDeltaTime);
        }

        if (_rotationInput != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, 
                Quaternion.Euler(transform.rotation.eulerAngles + _rotationInput * _tiltAngle), 
                _rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
