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
    [SerializeField] private CueCollider _cueCollider;
            
    private bool _isCharging;
    private bool _isChargeIncreasing;
    private bool _isShooting;
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
        _isChargeIncreasing = false;
        _isShooting = false;
        
        Messenger.AddListener<ShootChargingStarted>(OnShootChargingStarted);
        Messenger.AddListener<ShootChargingFinished>(OnShootChargingFinished);
    }

    private void Update()
    {
        if (_isCharging)
        {
            ForceMagnitude += (_isChargeIncreasing ?
                _globalConfiguration.ForceChargeVelocity :
                -_globalConfiguration.ForceChargeVelocity) * Time.deltaTime;

            if (ForceMagnitude >= _globalConfiguration.MaxCueForceMagnitude)
            {
                _isChargeIncreasing = false;
            }
            else if (ForceMagnitude <= 0f)
            {
                _isChargeIncreasing = true;
            }
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
    }

    public void DebugShoot(float forceMagnitude)
    {
        ForceMagnitude = forceMagnitude;
        ShootWithDelay();
    }

    public void StartNewTurn()
    {
        ForceMagnitude = 0f;
        _isShooting = false;
        _trajectoryRoot.gameObject.SetActive(true);
        _trajectoryHitPoint.gameObject.SetActive(true);
        _cueCollider.StartNewTurn();
    }

    public void End()
    {
        Messenger.RemoveListener<ShootChargingStarted>(OnShootChargingStarted);
        Messenger.RemoveListener<ShootChargingFinished>(OnShootChargingFinished);
    }

    private void OnShootChargingStarted(ShootChargingStarted msg)
    {
        _isCharging = true;
        _isChargeIncreasing = true;
    }

    private void OnShootChargingFinished(ShootChargingFinished msg)
    {
        _isCharging = false;

        ShootWithDelay();
    }
}
