using Messaging;
using UnityEngine;

[RequireComponent(typeof(Ball))]
[RequireComponent(typeof(Rigidbody))]
public class BallCollider : MonoBehaviour
{
    private Ball _thisBall;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _thisBall = GetComponent<Ball>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        var otherBall = other.gameObject.GetComponent<Ball>();
        if (otherBall == null)
        {
            return;
        }
    
        Messenger.Send(new BallCollidedWithBall(_thisBall, _rigidbody.velocity.sqrMagnitude, otherBall));
    }
}