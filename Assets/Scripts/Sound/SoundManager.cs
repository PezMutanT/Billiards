using System.Collections.Generic;
using Messaging;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _audioClips;
    
    public void Init()
    {
        Messenger.AddListener<BallCollidedWithBall>(OnBallCollidedWithBall);
    }

    public void End()
    {
        Messenger.RemoveListener<BallCollidedWithBall>(OnBallCollidedWithBall);
    }

    private void OnBallCollidedWithBall(BallCollidedWithBall msg)
    {
        _audioSource.clip = GetClipDependingOnVelocity(msg.BallAVelocitySqrMagnitude);   
        
        _audioSource.Play();
    }

    private AudioClip GetClipDependingOnVelocity(float ballVelocitySqrMagnitude)
    {
        //TODO 
        
        if (ballVelocitySqrMagnitude > 100f)
        {
            return _audioClips[1];
        }
        
        return _audioClips[0];
    }
}
