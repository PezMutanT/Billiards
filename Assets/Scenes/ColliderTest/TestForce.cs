using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TestForce : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    private Vector3 _velocity;
    [SerializeField] private float _forceMax;
    private float _forceMagnitude;
    private bool _hasHitBall;
    private int _markedToStopReally;
    [SerializeField] private int _numberOfFramesToWait;

    // Start is called before the first frame update
    void Start()
    {
        _velocity = Vector3.zero;
        _hasHitBall = false;
        _markedToStopReally = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"Update - _rigidbody.velocity:{_rigidbody.velocity}");
        if (Input.GetKeyDown(KeyCode.R))
        {
            Start();
        }
        
        ProcessShoot();
    }

    private void FixedUpdate()
    {
        //Debug.Log($"FixedUpdate - _rigidbody.velocity:{_rigidbody.velocity}");
        transform.position += _velocity * Time.deltaTime;

        /*if (_markedToStopReally >= _numberOfFramesToWait)
        {
            StopCylinder();

        }
        if (_hasHitBall)
        {
            _markedToStopReally++;
        }*/
    }

    private void Shoot(float velocity)
    {
        //_velocity = transform.up * velocity;
        
        //Debug.Log($"Adding force {velocity} to rigid body");
        _rigidbody.AddForce(-velocity, 0f, 0f);
    }

    private void ProcessShoot()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _forceMagnitude = _forceMax;
            Shoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _forceMagnitude = _forceMax * 0.1f;
            Shoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _forceMagnitude = _forceMax * 0.2f;
            Shoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _forceMagnitude = _forceMax * 0.3f;
            Shoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _forceMagnitude = _forceMax * 0.4f;
            Shoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _forceMagnitude = _forceMax * 0.5f;
            Shoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            _forceMagnitude = _forceMax * 0.6f;
            Shoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            _forceMagnitude = _forceMax * 0.7f;
            Shoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            _forceMagnitude = _forceMax * 0.8f;
            Shoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            _forceMagnitude = _forceMax * 0.9f;
            Shoot(_forceMagnitude);
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log($"OnCollisionEnter {gameObject.name} collided with {other.gameObject.name}");
        //Debug.Log($"OnCollisionEnter - _rigidbody.velocity:{_rigidbody.velocity}");

        // _hasHitBall = true;
        StopCylinder();
    }

    private void StopCylinder()
    {
        _rigidbody.velocity = Vector3.zero;;
        _velocity = Vector3.zero;
    }
}
