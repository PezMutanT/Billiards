using Messaging;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameHUD _gameHUD;
    [SerializeField] private OrbitAroundTarget _mainCamera;
    [SerializeField] private GameSetup _gameSetup;
    [SerializeField] private Cue _cue;

    private GameRules _gameRules;
    private int _ballsMovingAmount;

    void Awake()
    {
        InitObjects();
        
        StartNewGame();
    }

    private void InitObjects()
    {
        _gameSetup.Init();

        _gameRules = new GameRules();
    }

    private void StartNewGame()
    {
        _ballsMovingAmount = 0;
        Messenger.AddListener<BallStartedMoving>(OnBallStartedMoving);
        Messenger.AddListener<BallStoppedMoving>(OnBallStoppedMoving);

        _gameRules.Init();
        _gameHUD.Init();
        _mainCamera.Init();
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
            _gameRules.StartNewTurn();
            _gameHUD.StartNewTurn();
            _mainCamera.StartNewTurn();
            _cue.StartNewTurn();
        }
    }

    private void OnDestroy()
    {
        EndGame();
    }

    private void EndGame()
    {
        _gameRules.End();
        
        Messenger.RemoveListener<BallStartedMoving>(OnBallStartedMoving);
        Messenger.RemoveListener<BallStoppedMoving>(OnBallStoppedMoving);
    }
}
