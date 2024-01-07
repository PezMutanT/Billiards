using DG.Tweening;
using Messaging;
using UnityEngine;

public class Cue : MonoBehaviour
{
    [SerializeField] private GlobalConfiguration _globalConfiguration;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Rigidbody _whiteBallRigidBody;
    [SerializeField] private float _distanceToWhiteBall;
    [SerializeField] private LineRenderer _whiteBallTrajectoryLine;
    [SerializeField] private LineRenderer _secondBallTrajectoryLine;
    [SerializeField] private Transform _projectedWhiteBall;
    [SerializeField] private float _maxTrajectoryLineLength;
    [SerializeField] private float _maxSecondBallTrajectoryLineLength;
    [SerializeField] private Transform _cueAnimationRoot;
    [SerializeField] private CueCollider _cueCollider;
            
    private bool _isCharging;
    private bool _isChargeIncreasing;
    private bool _isShooting;
    private float _forceMagnitude;
    private float _ballRadius;

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
        
        _ballRadius =
            _whiteBallRigidBody.gameObject.GetComponent<SphereCollider>().radius * _whiteBallRigidBody.transform.localScale.x;
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
        
        if (Physics.SphereCast(
                whiteBallPosition,
                _ballRadius,
                transform.forward,
                out var hit,
                40f,
                LayerMask.GetMask("RaycastBalls", "Table")))
        {
            _projectedWhiteBall.position = whiteBallPosition + transform.forward * hit.distance;

            _whiteBallTrajectoryLine.SetPosition(0, whiteBallPosition);
            _whiteBallTrajectoryLine.SetPosition(1, whiteBallPosition + transform.forward * hit.distance);
            
            var secondBall = hit.collider.GetComponent<Ball>();
            if (secondBall == null)
            {
                _secondBallTrajectoryLine.gameObject.SetActive(false);
                return;
            }

            _secondBallTrajectoryLine.gameObject.SetActive(true);
            
            var secondBallPosition = secondBall.transform.position;
            var secondBallDirection = (secondBallPosition - hit.point).normalized;
            var secondTrajectoryStartingPosition = secondBallPosition + _ballRadius * secondBallDirection;
            
            _secondBallTrajectoryLine.SetPosition(0, secondTrajectoryStartingPosition);
            var lineLength = Mathf.Clamp(_maxTrajectoryLineLength - hit.distance, 0f, _maxSecondBallTrajectoryLineLength);
            _secondBallTrajectoryLine.SetPosition(1, secondTrajectoryStartingPosition + secondBallDirection * lineLength);

            if (Physics.SphereCast(
                    secondBallPosition,
                    _ballRadius,
                    secondBallDirection,
                    out var secondBallHit,
                    lineLength,
                    LayerMask.GetMask("RaycastBalls", "Table")))
            {
                _secondBallTrajectoryLine.SetPosition(1, secondBallHit.point);
            }
        }
    }

    private void ShootWithDelay()
    {
        _isShooting = true;
        
        _whiteBallTrajectoryLine.gameObject.SetActive(false);
        _projectedWhiteBall.gameObject.SetActive(false);

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
        _cueAnimationRoot.DORewind();
        _cueAnimationRoot.DOKill();

        ForceMagnitude = 0f;
        _isShooting = false;
        _projectedWhiteBall.gameObject.SetActive(true);
        _whiteBallTrajectoryLine.gameObject.SetActive(true);
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
