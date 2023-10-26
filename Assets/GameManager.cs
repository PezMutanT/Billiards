using Messaging;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameHUD _gameHUD;
    [SerializeField] private OrbitAroundTarget _mainCamera;
    [SerializeField] private GameSetup _gameSetup;
    [SerializeField] private Cue _cue;
    
    private int _ballsMovingAmount;

    void Awake()
    {
        InitObjects();
        
        StartNewGame();
    }

    private void InitObjects()
    {
        _gameSetup.Init();
        _mainCamera.Init();
    }

    private void StartNewGame()
    {
        _ballsMovingAmount = 0;
        Messenger.AddListener<BallStartedMoving>(OnBallStartedMoving);
        Messenger.AddListener<BallStoppedMoving>(OnBallStoppedMoving);

        _gameHUD.Init();
        _cue.Init();
    }

    private void OnBallStartedMoving(BallStartedMoving e)
    {
        _ballsMovingAmount++;
    }

    private void OnBallStoppedMoving(BallStoppedMoving e)
    {
        _ballsMovingAmount--;
        if (_ballsMovingAmount == 0)
        {
            Messenger.Send(new AllBallsStoppedMoving());
        }
    }

    private void OnDestroy()
    {
        EndGame();
    }

    private void EndGame()
    {
        Messenger.RemoveListener<BallStartedMoving>(OnBallStartedMoving);
        Messenger.RemoveListener<BallStoppedMoving>(OnBallStoppedMoving);
    }
}
