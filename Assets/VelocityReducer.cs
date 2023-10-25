using Messaging;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Ball))]
public class VelocityReducer : MonoBehaviour
{
    [SerializeField] private GlobalConfiguration _globalConfiguration;
    
    
    private Rigidbody _rigidbody;
    private Ball _thisBall;
    private bool _isBallMoving;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _thisBall = GetComponent<Ball>();
        
        _isBallMoving = false;
        
        Messenger.AddListener<BallStartedMoving>(OnBallStartedMoving);
    }

    private void OnBallStartedMoving(BallStartedMoving e)
    {
        if (e.Ball != _thisBall)
        {
            return;
        }

        _isBallMoving = true;
    }

    void FixedUpdate()
    {
        if (!_isBallMoving)
        {
            return;
        }
        
        if (_rigidbody.velocity.sqrMagnitude <= _globalConfiguration.BallMinVelocityThreshold)
        {
            Debug.Log($"{gameObject.name} min velocity threshold triggered at {_rigidbody.velocity.x:0.0000},{_rigidbody.velocity.y:0.0000},{_rigidbody.velocity.z:0.0000}");
            _isBallMoving = false;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener<BallStartedMoving>(OnBallStartedMoving);
    }
}
