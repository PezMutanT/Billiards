using System.Collections;
using Messaging;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private BallConfiguration _ballConfiguration;
    [SerializeField] private Rigidbody _rigidbody;

    public BallType BallType => _ballConfiguration.BallType;
    public int ScoreWhenPotted => _ballConfiguration.ScoreWhenPotted;

    private Vector3 _initialPosition;
    private Vector3 _previousVelocity;
    private bool _isInitializing;
    
    public void Init()
    {
        ApplyBallColor();

        _initialPosition = transform.position;
        _previousVelocity = Vector3.zero;

        Messenger.AddListener<BallEnteredPot>(OnBallEnteredPot);

        //StartCoroutine(WaitUntilBallIsStill());
    }

    private void ApplyBallColor()
    {
        var renderer = GetComponent<Renderer>();
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetColor("_BaseColor", _ballConfiguration.Color);
        renderer.SetPropertyBlock(propertyBlock);
    }

    private IEnumerator WaitUntilBallIsStill()
    {
        _isInitializing = true;
        
        var waitCoroutine = new WaitForSeconds(1.0f);
        yield return waitCoroutine;

        _isInitializing = false;
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener<BallEnteredPot>(OnBallEnteredPot);
    }

    private void OnBallEnteredPot(BallEnteredPot e)
    {
        if (e.Ball != this)
        {
            return;
        }

        RemoveFromGame();
        Messenger.Send(new BallStoppedMoving(this));
    }

    private void RemoveFromGame()
    {
        ForceStop();
        gameObject.SetActive(false);
    }

    public void DebugReset()
    {
        ForceStop();
        transform.position = _initialPosition;
    }

    private void ForceStop()
    {
        _previousVelocity = Vector3.zero;
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void Update()
    {
        if (_isInitializing)
        {
            return;
        }
        
        if (_previousVelocity == Vector3.zero && _rigidbody.velocity != Vector3.zero)
        {
            // Debug.Log($"Ball {name} started moving");
            Messenger.Send(new BallStartedMoving(this));
        }
        else if (_previousVelocity != Vector3.zero && _rigidbody.velocity == Vector3.zero)
        {
            // Debug.Log($"Ball {name} stopped moving");
            Messenger.Send(new BallStoppedMoving(this));
        }

        _previousVelocity = _rigidbody.velocity;
    }

    public void Respot()
    {
        gameObject.SetActive(true);
        ForceStop();
        transform.position = _initialPosition;
        _rigidbody.useGravity = true;
    }

    public void AccelerateFall()
    {
        _rigidbody.AddForce(Vector3.down * 20f);
    }
}