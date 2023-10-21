using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableFrictionTest : MonoBehaviour
{
    public TableFrictionTestConfiguration _tableFrictionTestConfiguration;
    public GameObject _ballPrefab;
    public Transform _ballSpawnerPlaceholder;
    public int _maxBalls;
    public float _ballOffset;
    
    private bool _isTopCameraOn = false;

    private List<Rigidbody> BallRigidBodies = new List<Rigidbody>();

    void Start()
    {
        var newBallPosition = _ballSpawnerPlaceholder.position;
        for (var i = 0; i < _maxBalls; i++)
        {
            var newBall = Instantiate(_ballPrefab, newBallPosition, Quaternion.identity);
            BallRigidBodies.Add(newBall.GetComponent<Rigidbody>());
            
            newBallPosition = new Vector3(newBallPosition.x, newBallPosition.y, newBallPosition.z + _ballOffset);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            var forceToAdd = _tableFrictionTestConfiguration.Force;
            foreach (var ballRigidBody in BallRigidBodies)
            {
                ballRigidBody.AddForce(
                    forceToAdd,
                    _tableFrictionTestConfiguration.ForceMode);

                forceToAdd = new Vector3(forceToAdd.x + _tableFrictionTestConfiguration.ForceDelta, forceToAdd.y, forceToAdd.z);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            DoReset();
        }
    }

    private void DoReset()
    {
        foreach (var ballRigidBody in BallRigidBodies)
        {
            Destroy(ballRigidBody.gameObject);
        }
        
        BallRigidBodies.Clear();
        
        Start();
    }
}
