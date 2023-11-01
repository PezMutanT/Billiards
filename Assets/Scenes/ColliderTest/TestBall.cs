using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBall : MonoBehaviour
{
    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log($"OnCollisionEnter {gameObject.name} collided with {other.gameObject.name}");
        //Debug.Log($"OnCollisionEnter - _rigidbody.velocity:{_rigidbody.velocity}");
    }
}
