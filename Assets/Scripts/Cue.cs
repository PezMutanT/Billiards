using Messaging;
using UnityEngine;

public class Cue : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Rigidbody _whiteBallRigidBody;
    [SerializeField] private float _distanceToWhiteBall;
    [SerializeField] private float _forceChargeOverTime;

    private bool _isCharging = false;
    private bool _isShooting = false;
    private float _forceMagnitude;
    
    private void Start()
    {
        Messenger.AddListener<AllBallsStoppedMoving>(OnAllBallsStoppedMoving);
        
        _isCharging = false;
        _isShooting = false;
    }

    private void OnAllBallsStoppedMoving(AllBallsStoppedMoving e)
    {
        _isShooting = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isCharging = true;
        }
        
        if (_isCharging)
        {
            _forceMagnitude += _forceChargeOverTime;
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _isCharging = false;

            Shoot();

            _forceMagnitude = 0f;
        }
    }

    public void UpdateFromCamera()
    {
        if (_isCharging || _isShooting)
        {
            return;
        }
        
        transform.forward = _cameraTransform.forward;

        var rotationEulerAngles = transform.rotation.eulerAngles;
        rotationEulerAngles.x = 0f;
        transform.rotation = Quaternion.Euler(rotationEulerAngles);

        var whiteBallPosition = _whiteBallRigidBody.transform.position;
        transform.position = whiteBallPosition - transform.forward.normalized * _distanceToWhiteBall;
    }

    private void Shoot()
    {
        _isShooting = true;
        _rigidBody.AddForce(transform.forward * _forceMagnitude);
    }

    private void OnCollisionEnter(Collision other)
    {
        var otherRigidBody = other.gameObject.GetComponent<Rigidbody>();
        if (otherRigidBody != null && otherRigidBody == _whiteBallRigidBody)
        {
            _rigidBody.velocity = Vector3.zero;
        }
    }
}
