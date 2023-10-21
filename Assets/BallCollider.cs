using System;
using Messaging;
using UnityEngine;

[RequireComponent(typeof(Ball))]
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
    
        Messenger.Send(new BallCollidedWithBall(_thisBall, otherBall));
    }

    private void Update()
    {
        //Debug.Log($"velocity: {_rigidbody.velocity}, angularVelocity: {_rigidbody.angularVelocity}");
    }
}