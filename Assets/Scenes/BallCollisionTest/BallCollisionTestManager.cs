using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private class BallData
    {
        public GameObject _gameObject;
        public Rigidbody _rigidBody => _gameObject.GetComponent<Rigidbody>();
        public SphereCollider _sphereCollider => _gameObject.GetComponent<SphereCollider>();
        public Vector3 _position;
        public PhysicMaterial _physicMaterial;
    }
    
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private int _ballAmount;
    [SerializeField] private float _frictionIncrement;
    [SerializeField] private float _bounciness;
    
    [SerializeField] private Vector3 _force;
    
    private List<BallData> _balls = new List<BallData>();

    void Awake()
    {
        CreateBalls();
    }

    private void CreateBalls()
    {
        for (int i = 0; i < _ballAmount; i++)
        {
            BallData ballData = new BallData();
            ballData._position = new Vector3(10f, 0f, (i - (_ballAmount / 2)) * 1.5f);
            ballData._gameObject = Instantiate(_ballPrefab, ballData._position, Quaternion.identity);
            ballData._physicMaterial = new PhysicMaterial();
            ballData._physicMaterial.staticFriction = _frictionIncrement * i;
            ballData._physicMaterial.dynamicFriction = _frictionIncrement * i;
            ballData._physicMaterial.bounciness = _bounciness;
            ballData._sphereCollider.material = ballData._physicMaterial;

            _balls.Add(ballData);
        }
    }

    private void Start()
    {
        //LaunchBalls();
    }

    private void LaunchBalls()
    {
        foreach (var ball in _balls)
        {
            ball._rigidBody.AddForce(_force);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            DoReset();
        }
    }

    private void DoReset()
    {
        DestroyBalls();
        CreateBalls();
        LaunchBalls();
    }

    private void DestroyBalls()
    {
        foreach (var ballData in _balls)
        {
            Destroy(ballData._gameObject);
        }
        
        _balls.Clear();
    }
}
