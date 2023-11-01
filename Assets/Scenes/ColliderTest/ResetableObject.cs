using UnityEngine;

public class ResetableObject : MonoBehaviour
{
    private Vector3 _initialPosition;
    private Rigidbody _rigidbody;
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        _initialPosition = transform.position;
    }

    public void DoReset()
    {
        if (_rigidbody)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        transform.position = _initialPosition;
    }
}
