using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StarshipController : MonoBehaviour
{
    [SerializeField] private float _speed = 30f;
    [SerializeField] private float _rotationSpeed = 2f;
    [SerializeField] private float _tiltAngle = 30f;

    private Rigidbody _rigidbody;
    private Vector3 _movementInput;
    private Vector3 _rotationInput;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        _movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized 
            + new Vector3(0, Input.GetAxis("Mouse Y"), 0);

        _rotationInput = new Vector3(0, Input.GetAxis("Mouse X"), 0);
    }

    private void FixedUpdate()
    {
        if (_movementInput != Vector3.zero)
        {
            Vector3 movementDirection = transform.forward * _movementInput.z + transform.right * _movementInput.x + transform.up * _movementInput.y;
            _rigidbody.velocity = movementDirection * _speed;

            Vector3 targetRotation = new Vector3(-_movementInput.y * _tiltAngle, transform.rotation.eulerAngles.y, -_movementInput.x * _tiltAngle);

            transform.rotation = Quaternion.Lerp(transform.rotation, 
                Quaternion.Euler(targetRotation), 
                _rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
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
