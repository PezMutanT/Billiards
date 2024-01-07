using Messaging;
using UnityEngine;

public class BallCollider : MonoBehaviour
{
    private Ball _thisBall;
    private Rigidbody _rigidbody;
    private bool _isGhost;

    public void Init(bool isGhost)
    {
        _thisBall = GetComponent<Ball>();
        _rigidbody = GetComponent<Rigidbody>();
        _isGhost = isGhost;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_isGhost)
        {
            return;
        }
        var otherBall = other.gameObject.GetComponent<Ball>();
        if (otherBall == null)
        {
            return;
        }
    
        Messenger.Send(new BallCollidedWithBall(_thisBall, _rigidbody.velocity.sqrMagnitude, otherBall));
    }
}