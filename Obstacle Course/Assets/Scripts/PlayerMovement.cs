using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _rotationSpeed = 360f;

    private Animator _animator;
    private Vector3 _velocity;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _velocity = Vector3.zero;
    }

    private void Update()
    {
        // read input and update velocity
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        _velocity.x = LerpClamp(_velocity.x, x);
        _velocity.z = LerpClamp(_velocity.z, z);
        Vector3 velocity = Vector3.ClampMagnitude(_velocity, 1f);

        _animator.SetFloat("Speed", velocity.magnitude);

        // move in the requested direction
        velocity = _velocity * Time.deltaTime * _speed * -1f;
        transform.Translate(velocity, Space.World);

        // face direction of movement (assuming there is movement)
        if (_velocity.magnitude > 0.08f)
        {
            Quaternion toRotation = Quaternion.LookRotation(velocity, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private float LerpClamp(float start, float target)
    {
        float deltaChange = Time.deltaTime * _acceleration;
        if (start > target)
        {
            if (start - deltaChange > target)
            {
                return start - deltaChange;
            } // else will return target on last line
        }
        else if (target > start)
        {
            if (start + deltaChange < target)
            {
                return start + deltaChange;
            } // else will return target on last line
        }
        return target;
    }
}
