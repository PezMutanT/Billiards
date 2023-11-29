using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsMaterialsTest : MonoBehaviour
{
    [SerializeField] private Rigidbody _ball1;
    [SerializeField] private Rigidbody _ball2;
    [SerializeField] private float _forceMagnitude;

    private Vector3 _ball1InitialPosition;
    private Vector3 _ball2InitialPosition;
    
    void Awake()
    {
        _ball1InitialPosition = _ball1.transform.position;
        _ball2InitialPosition = _ball2.transform.position;
    }

    private void StopRigidbodies()
    {
        _ball1.velocity = Vector3.zero;
        _ball1.angularVelocity = Vector3.zero;

        _ball2.velocity = Vector3.zero;
        _ball2.angularVelocity = Vector3.zero;
    }

    private void Shoot()
    {
        _ball2.AddForce(new Vector3(-_forceMagnitude, 0f, 0f), ForceMode.Impulse);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Shoot();
        }
    }

    private void Reset()
    {
        _ball1.transform.position = _ball1InitialPosition;
        _ball2.transform.position = _ball2InitialPosition;
        StopRigidbodies();
    }
}
