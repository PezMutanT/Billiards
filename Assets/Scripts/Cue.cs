using Messaging;
using UnityEngine;

public class Cue : MonoBehaviour
{
    [SerializeField] private GlobalConfiguration _globalConfiguration;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Rigidbody _whiteBallRigidBody;
    [SerializeField] private float _distanceToWhiteBall;
    [SerializeField] private float _forceChargeOverTime;

    private bool _isCharging = false;
    private bool _isShooting = false;
    private float _forceMagnitude;
    
    public void Init()
    {
        Messenger.AddListener<AllBallsStoppedMoving>(OnAllBallsStoppedMoving);
        
        _isCharging = false;
        _isShooting = false;
    }

    private void OnAllBallsStoppedMoving(AllBallsStoppedMoving e)
    {
        _isShooting = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 60f);
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
        }
    }

    public void UpdateFromCamera(Vector3 cameraForwardVector)
    {
        if (_isCharging || _isShooting)
        {
            return;
        }
        
        transform.forward = cameraForwardVector;

        var rotationEulerAngles = transform.rotation.eulerAngles;
        rotationEulerAngles.x = 0f;
        transform.rotation = Quaternion.Euler(rotationEulerAngles);

        var whiteBallPosition = _whiteBallRigidBody.transform.position;
        transform.position = whiteBallPosition - transform.forward.normalized * _distanceToWhiteBall;
    }

    private void Shoot()
    {
        _isShooting = true;
        _rigidBody.AddForce(transform.forward * Mathf.Max(_forceMagnitude, _globalConfiguration.MinCueForceMagnitude));
        _forceMagnitude = 0f;
    }

    private void OnCollisionEnter(Collision other)
    {
        var otherRigidBody = other.gameObject.GetComponent<Rigidbody>();
        if (otherRigidBody != null && otherRigidBody == _whiteBallRigidBody)
        {
            _rigidBody.velocity = Vector3.zero;
        }
    }

    public void DebugShoot(float forceMagnitude)
    {
        _forceMagnitude = forceMagnitude;
        Shoot();
    }
}
