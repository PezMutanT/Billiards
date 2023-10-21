using System;
using UnityEngine;

public class CueOLD : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Rigidbody _whiteBallRigidBody;
    [SerializeField] private float _initialOffset;
    [SerializeField] private float _forceChargeOverTime;

    private bool _isCharging = false;
    private float _forceMagnitude;
    private Vector3 _thisInitialTransform;
    private Vector3 _thisInitialRotation;
    private Vector3 _whiteInitialTransform;
    private Vector3 _debugDirection;
    private bool _isShooting;

    private void Awake()
    {
        _thisInitialTransform = transform.position;
        _thisInitialRotation = transform.eulerAngles;
        _whiteInitialTransform = _whiteBallRigidBody.transform.position;
        _isShooting = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.forward * 50f);
    }

    private void Update()
    {
        if (_isShooting)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            _rigidBody.gameObject.SetActive(false);
            _whiteBallRigidBody.gameObject.SetActive(false);
            _rigidBody.velocity = Vector3.zero;
            _whiteBallRigidBody.velocity = Vector3.zero;
            _whiteBallRigidBody.angularVelocity = Vector3.zero;
            transform.position = _thisInitialTransform;
            _whiteBallRigidBody.transform.position = _whiteInitialTransform;
            _rigidBody.gameObject.SetActive(true);
            _whiteBallRigidBody.gameObject.SetActive(true);
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("_isCharging = true");
            _isCharging = true;
        }
        
        if (_isCharging)
        {
            _forceMagnitude += _forceChargeOverTime;
            Debug.Log($"_forceMagnitude: {_forceMagnitude}");
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("_isCharging = false");
            _isCharging = false;

            Shoot();

            _forceMagnitude = 0f;
        }

        var cameraRotationEulerAngles = _cameraTransform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, cameraRotationEulerAngles.y, cameraRotationEulerAngles.z);

        PrepareBehindWhiteBall(_cameraTransform.forward.normalized);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, _debugDirection * 100.0f);
    }

    public void PrepareBehindWhiteBall(Vector3 direction)
    {
        //Debug.Log($"direction : {direction}");
        _debugDirection = direction;
        
        //_rigidBody.velocity = Vector3.zero;
        //_rigidBody.gameObject.SetActive(false);
        var newPosition = _whiteBallRigidBody.transform.position - _initialOffset * direction;
        newPosition.y = 0f;
        transform.position = newPosition;
        
        
        direction.x = 0f;
        //transform.forward = direction;
        // _rigidBody.gameObject.SetActive(true);
    }

    private void Shoot()
    {
        _isShooting = true;
        
        _rigidBody.AddRelativeForce(transform.forward * _forceMagnitude);
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
