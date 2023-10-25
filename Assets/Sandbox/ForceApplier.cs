using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceApplier : MonoBehaviour
{
    private Rigidbody rb;
    private bool _isBallMoving;
    private bool _hasToStop;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isBallMoving && rb.velocity.sqrMagnitude <= 0.005f)
        {
            _hasToStop = true;
            _isBallMoving = false;
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            _isBallMoving = true;
            rb.AddForce(100f, 0f, 0f);

        }

        Debug.Log($"vel: {rb.velocity}, ang: {rb.angularVelocity}");
    }

    private void FixedUpdate()
    {
        if (_hasToStop)
        {
            _isBallMoving = false;
            _hasToStop = false;
            Debug.Log($"STOP");
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
