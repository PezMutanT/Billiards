using UnityEngine;

public class DebugInput : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            _gameManager.ResetGame();
        }
    }
}
