using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameHUD _gameHUD;
    
    void Start()
    {
        _gameHUD.Init();    
    }
}
