using System.Collections.Generic;
using Messaging;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameHUD _gameHUD;
    [SerializeField] private CameraDirector _cameraDirector;
    [SerializeField] private GameSetup _gameSetup;
    [SerializeField] private Cue _cue;
    [SerializeField] private Ball _whiteBall;

    private GameRules _gameRules;
    private int _ballsMovingAmount;

    private List<Ball> AllBalls => _gameSetup.AllBalls;

    void Awake()
    {
        Init();
    }

    private void Init()
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
        _cameraDirector.Init();
        
        _ballsMovingAmount = 0;
        Messenger.AddListener<BallStartedMoving>(OnBallStartedMoving);
        Messenger.AddListener<BallStoppedMoving>(OnBallStoppedMoving);
        Messenger.AddListener<PlayerAnnouncedShot>(OnPlayerAnnouncedShot);

        _gameRules.Init(AllBalls);
        _gameHUD.Init();
        _cue.Init();
        _whiteBall.Init();
    }

    private void OnPlayerAnnouncedShot(PlayerAnnouncedShot e)
    {
        _cameraDirector.SwitchToShotCamera(e.CueBall, e.Direction);
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
            _gameRules.CheckScoreThisTurn();
            
            _gameRules.StartNewTurn();
            _gameHUD.StartNewTurn();
            _cameraDirector.StartNewTurn(_gameRules.NextBallOnPosition);
            _cue.StartNewTurn();
        }
    }

    private void OnDestroy()
    {
        EndGame();
    }

    private void EndGame()
    {
        _cue.End();
        _gameHUD.End();
        _gameRules.End();
        
        Messenger.RemoveListener<BallStartedMoving>(OnBallStartedMoving);
        Messenger.RemoveListener<BallStoppedMoving>(OnBallStoppedMoving);
        Messenger.RemoveListener<PlayerAnnouncedShot>(OnPlayerAnnouncedShot);

        _cameraDirector.End();
    }

    public void DebugResetGame()
    {
        EndGame();
        
        Init();
    }
}
