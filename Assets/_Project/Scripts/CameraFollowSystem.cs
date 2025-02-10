using UnityEngine;

public class CameraFollowSystem : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _distance = 7f;
    [SerializeField] private float _height = 5f;
    [SerializeField] private float _positionSmoothTime = 0.2f;
    [SerializeField] private float _rotationSmoothTime = 0.1f;

    private Vector3 _offset;
    private Vector3 _currentVelocity;
    private float _rotationVelocity;

    private void Start()
    {
        _offset = new Vector3(0, _height, -_distance);
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = _target.position + _target.TransformDirection(_offset);

        transform.position = Vector3.SmoothDamp(new Vector3(transform.position.x, 0, transform.position.z), desiredPosition, ref _currentVelocity, _positionSmoothTime);
        transform.position = new Vector3(transform.position.x, desiredPosition.y, transform.position.z);

        Quaternion desiredRotation = Quaternion.LookRotation(_target.position - transform.position, _target.up);

        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, desiredRotation.eulerAngles.y, ref _rotationVelocity, _rotationSmoothTime),
            transform.rotation.eulerAngles.z
        );
    }
}
