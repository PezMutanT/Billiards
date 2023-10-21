using System;
using Messaging;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private GlobalConfiguration _globalConfiguration;
    [SerializeField] private BallConfiguration _ballConfiguration;
    [SerializeField] private Rigidbody _rigidbody;

    public BallType BallType => _ballConfiguration.BallType;
    public int ScoreWhenPotted => _ballConfiguration.ScoreWhenPotted;

    private Vector3 _previousVelocity;
    
    public void Start()
    {
        /*_rigidbody = GetComponent<Rigidbody>();
        _rigidbody.sleepThreshold = _globalConfiguration.BallsSleepThreshold;*/
        
        ApplyBallColor();

        _previousVelocity = Vector3.zero;
    }

    private void ApplyBallColor()
    {
        var renderer = GetComponent<Renderer>();
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetColor("_BaseColor", _ballConfiguration.Color);
        renderer.SetPropertyBlock(propertyBlock);
    }

    private void Update()
    {
        //_rigidbody.sleepThreshold = _globalConfiguration.BallsSleepThreshold;

        if (_previousVelocity == Vector3.zero && _rigidbody.velocity != Vector3.zero)
        {
            Debug.Log($"Ball {name} started moving");
            Messenger.Send(new BallStartedMoving(this));
        }
        else if (_previousVelocity != Vector3.zero && _rigidbody.velocity == Vector3.zero)

        {
            Debug.Log($"Ball {name} stopped moving");
            Messenger.Send(new BallStoppedMoving(this));
        }

        _previousVelocity = _rigidbody.velocity;
    }
}