using Messaging;
using UnityEngine;

public class Pot : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var otherBall = other.gameObject.GetComponent<Ball>();
        if (otherBall == null)
        {
            Debug.LogError($"Collision: {other.gameObject.name} with {gameObject.name}");
            return;
        }
        
        Debug.Log($"Ball {otherBall.name} entered pot {gameObject.name}");
        Messenger.Send(new BallEnteredPot(otherBall, this));
    }
}
