using UnityEngine;

public class PotTrigger : MonoBehaviour
{
    [SerializeField] private Pot _pot;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collision: {other.gameObject.name} with {gameObject.name}");
        
        var otherBall = other.gameObject.GetComponent<Ball>();
        if (otherBall == null)
        {
            Debug.LogError($"Collision: {other.gameObject.name} with {gameObject.name}");
            return;
        }
        
        _pot.OnBallEnteredPot(otherBall);
    }
}
