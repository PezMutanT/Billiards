using System;
using System.Collections.Generic;
using Messaging;
using UnityEngine;

[Serializable]
public class AudioClipInfo
{
    public float MinBallVelocity;
    public AudioClip AudioClip;

    public AudioClipInfo(float minBallVelocity, AudioClip audioClip)
    {
        MinBallVelocity = minBallVelocity;
        AudioClip = audioClip;
    }
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClipInfo> _audioClips;
    
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
        var audioClip = GetClipDependingOnVelocity(msg.BallAVelocitySqrMagnitude);
        if (audioClip == null)
        {
            return;
        }
        
        _audioSource.clip = audioClip;   
        
        _audioSource.Play();
    }

    private AudioClip GetClipDependingOnVelocity(float ballVelocitySqrMagnitude)
    {
        AudioClip resultAudioClip = null;
        foreach (var audioClipInfo in _audioClips)
        {
            if (ballVelocitySqrMagnitude >= audioClipInfo.MinBallVelocity)
            {
                resultAudioClip = audioClipInfo.AudioClip;
                continue;
            }

            break;
        }

        return resultAudioClip;
    }
}
