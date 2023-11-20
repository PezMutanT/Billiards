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
    [SerializeField] private Transform _trajectoryRoot;
    
    private bool _isCharging = false;
    private bool _isShooting = false;
    private float _forceMagnitude;
    private float ForceMagnitude
    {
        get => _forceMagnitude;
        set
        {
            Messenger.Send(new ShootForceMagnitudeChanged(value));
            _forceMagnitude = value;
        }
    }

    public void Init()
    {
        _isCharging = false;
        _isShooting = false;
        
        Messenger.AddListener<ShootChargingStarted>(OnShootChargingStarted);
        Messenger.AddListener<ShootChargingFinished>(OnShootChargingFinished);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 60f);
    }

    private void Update()
    {
        if (_isCharging)
        {
            ForceMagnitude += _forceChargeOverTime;
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
        
        
        
        //sphere cast
        var ballRadius = _whiteBallRigidBody.gameObject.GetComponent<SphereCollider>().radius * _whiteBallRigidBody.transform.localScale.x;
        if (Physics.SphereCast(
                _whiteBallRigidBody.transform.position,
                ballRadius,
                transform.forward,
                out var hit,
                40f,
                LayerMask.GetMask("RaycastBalls", "Table")))
        {
            _trajectoryRoot.localScale = new Vector3(1f, 1f, hit.distance);
        }
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
            ForceMagnitude,
            _globalConfiguration.MinCueForceMagnitude,
            _globalConfiguration.MaxCueForceMagnitude);
        
        Debug.Log($"Shooting with force magnitude: {forceMagnitude}");
        _isShooting = true;
        _rigidBody.AddForce(transform.forward * forceMagnitude);
        
        ForceMagnitude = 0f;
    }

    public void DebugShoot(float forceMagnitude)
    {
        ForceMagnitude = forceMagnitude;
        StartCoroutine(ShootWithDelay());
    }

    public void StartNewTurn()
    {
        _isShooting = false;
    }

    public void End()
    {
        Messenger.RemoveListener<ShootChargingStarted>(OnShootChargingStarted);
        Messenger.RemoveListener<ShootChargingFinished>(OnShootChargingFinished);
    }

    private void OnShootChargingStarted(ShootChargingStarted msg)
    {
        _isCharging = true;
    }

    private void OnShootChargingFinished(ShootChargingFinished msg)
    {
        _isCharging = false;

        StartCoroutine(ShootWithDelay());
    }
}
