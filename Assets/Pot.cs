using Messaging;
using UnityEngine;

public class Pot : MonoBehaviour
{
    [SerializeField] private PotConfiguration _potConfiguration;
    
    public void OnBallEnteredPot(Ball ball)
    {
        Messenger.Send(new BallCollidedWithPot(ball, this));
    }
}
