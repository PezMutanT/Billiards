using System.Collections;
using UnityEngine;

public class CueCollider : MonoBehaviour
{
    [SerializeField] private Rigidbody _thisRigidBody;
    [SerializeField] private Rigidbody _whiteBallRigidBody;

    private Vector3 _initialLocalPosition;
    
    private void Awake()
    {
        _initialLocalPosition = transform.localPosition;
    }

    private void OnCollisionEnter(Collision other)
    {
        var otherRigidBody = other.gameObject.GetComponent<Rigidbody>();
        if (otherRigidBody != null && otherRigidBody == _whiteBallRigidBody)
        {
            Debug.Log($"Kike - Collided");
            _thisRigidBody.velocity = Vector3.zero;
            
            StartCoroutine(ResetLocalPosition());
        }
    }

    private IEnumerator ResetLocalPosition()
    {
        yield return new WaitForSeconds(0.5f);
        transform.localPosition = _initialLocalPosition;
    }
}
