using System.Collections;
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
        _isCharging = false;
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

            StartCoroutine(ShootWithDelay());
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

    private IEnumerator ShootWithDelay()
    {
        Messenger.Send(new PlayerAnnouncedShot(_whiteBallRigidBody.transform, transform.forward));
        
        yield return new WaitForSeconds(_globalConfiguration.ShootDelaySeconds);

        Shoot();
    }

    private void Shoot()
    {
        var forceMagnitude = Mathf.Clamp(
            _forceMagnitude,
            _globalConfiguration.MinCueForceMagnitude,
            _globalConfiguration.MaxCueForceMagnitude);
        
        Debug.Log($"Shooting with force magnitude: {forceMagnitude}");
        _isShooting = true;
        _rigidBody.AddForce(transform.forward * forceMagnitude);
        
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
        StartCoroutine(ShootWithDelay());
    }

    public void StartNewTurn()
    {
        _isShooting = false;
    }
}
