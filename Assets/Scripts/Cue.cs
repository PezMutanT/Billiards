using System.Collections;
using DG.Tweening;
using Messaging;
using UnityEngine;

public class Cue : MonoBehaviour
{
    [SerializeField] private GlobalConfiguration _globalConfiguration;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Rigidbody _whiteBallRigidBody;
    [SerializeField] private float _distanceToWhiteBall;
    [SerializeField] private Transform _trajectoryRoot;
    [SerializeField] private Transform _trajectoryHitPoint;
    [SerializeField] private Transform _cueAnimationRoot;
        
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

    private void Update()
    {
        if (_isCharging)
        {
            ForceMagnitude += _globalConfiguration.ForceChargeVelocity * Time.deltaTime;
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
            _trajectoryHitPoint.position = whiteBallPosition + transform.forward * hit.distance;
        }
    }

    private void ShootWithDelay()
    {
        _isShooting = true;
        
        _trajectoryRoot.gameObject.SetActive(false);
        _trajectoryHitPoint.gameObject.SetActive(false);

        Messenger.Send(new PlayerAnnouncedShot(_whiteBallRigidBody.transform, transform.forward));
        
        _cueAnimationRoot.DOMove(_cueAnimationRoot.position + _cueAnimationRoot.forward * (_distanceToWhiteBall - 0.4f), 0.5f)
            .SetLoops(4, LoopType.Yoyo)
            .OnComplete(Shoot);
    }

    private void Shoot()
    {
        var forceMagnitude = Mathf.Clamp(
            ForceMagnitude,
            _globalConfiguration.MinCueForceMagnitude,
            _globalConfiguration.MaxCueForceMagnitude);
        
        Debug.Log($"Shooting with force magnitude: {forceMagnitude}");
        _rigidBody.AddForce(transform.forward * forceMagnitude);
        
        ForceMagnitude = 0f;
    }

    public void DebugShoot(float forceMagnitude)
    {
        ForceMagnitude = forceMagnitude;
        ShootWithDelay();
    }

    public void StartNewTurn()
    {
        _isShooting = false;
        _trajectoryRoot.gameObject.SetActive(true);
        _trajectoryHitPoint.gameObject.SetActive(true);
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

        ShootWithDelay();
    }
}
