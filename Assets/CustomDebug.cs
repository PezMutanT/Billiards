using UnityEngine;

public class CustomDebug : MonoBehaviour
{
    private GameSetup _gameSetup;
    
    void Awake()
    {
        _gameSetup = GetComponent<GameSetup>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            _gameSetup.SetupBalls();
        }
    }
}
