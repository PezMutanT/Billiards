using UnityEngine;
using DG.Tweening;

public class CueCollider : MonoBehaviour
{
    [SerializeField] private Rigidbody _thisRigidBody;
    [SerializeField] private Rigidbody _whiteBallRigidBody;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Vector3 _outOfSightPosition;
    
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
            _thisRigidBody.velocity = Vector3.zero;
            
            _audioSource.Play();

            _thisRigidBody.detectCollisions = false;
            
            transform.DOMove(_outOfSightPosition, 0.7f)
                .SetDelay(0.3f)
                .SetEase(Ease.InSine);
        }
    }

    private void ResetLocalPosition()
    {
        transform.localPosition = _initialLocalPosition;
    }

    public void StartNewTurn()
    {
        ResetLocalPosition();
        _thisRigidBody.detectCollisions = true;
    }
}
