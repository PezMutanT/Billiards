using System.Collections.Generic;
using DG.Tweening;
using Messaging;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private GameHUD _gameHUD;
    [SerializeField] private FoulText _foulText;
    [SerializeField] private CameraDirector _cameraDirector;
    [SerializeField] private GameSetup _gameSetup;
    [SerializeField] private Cue _cue;
    [SerializeField] private Ball _whiteBall;

    private GameRules _gameRules;
    private int _ballsMovingAmount;
    private bool _isFirstBallCollisionInTurn;

    private List<Ball> AllBalls => _gameSetup.AllBalls;

    void Awake()
    {
        DOTween.Init();

        Init();
    }

    private void Init()
    {
        _cameraDirector.Init();
        StartNewGame();
    }

    private void StartNewGame()
    {
        _soundManager.Init();

        _gameRules = new GameRules();
        _gameSetup.Init();
        
        _ballsMovingAmount = 0;
        Messenger.AddListener<BallStartedMoving>(OnBallStartedMoving);
        Messenger.AddListener<BallStoppedMoving>(OnBallStoppedMoving);
        Messenger.AddListener<PlayerAnnouncedShot>(OnPlayerAnnouncedShot);
        Messenger.AddListener<BallCollidedWithBall>(OnBallCollidedWithBall);

        _gameRules.Init(AllBalls);
        _gameHUD.Init(_gameRules.AllowedBallTypes);
        _cue.Init();
        _whiteBall.Init();
        
        _isFirstBallCollisionInTurn = true;
    }

    private void OnPlayerAnnouncedShot(PlayerAnnouncedShot e)
    {
        _cameraDirector.SwitchToShotCamera(e.CueBall, e.Direction);
    }

    private void OnBallStartedMoving(BallStartedMoving e)
    {
        _ballsMovingAmount++;
        
        Debug.Log($"Balls moving (increased): {_ballsMovingAmount}");
    }

    private void OnBallStoppedMoving(BallStoppedMoving e)
    {
        _ballsMovingAmount--;
        
        Debug.Log($"Balls moving (decreased): {_ballsMovingAmount}");
        
        if (_ballsMovingAmount == 0)
        {
            _gameRules.CheckScoreThisTurn();
            StartNewTurn();
        }
    }

    private void StartNewTurn()
    {
        _gameRules.StartNewTurn();
        _gameHUD.StartNewTurn(_gameRules.AllowedBallTypes);
        _cameraDirector.StartNewTurn(_gameRules.NextBallOnPosition);
        _cue.StartNewTurn();
        _isFirstBallCollisionInTurn = true;
    }

    private void OnBallCollidedWithBall(BallCollidedWithBall msg)
    {
        var ballABallType = msg.BallA.BallType;
        var ballBBallType = msg.BallB.BallType;
        
        Debug.Log($"Ball {ballABallType} collided with ball {ballBBallType}");

        if (!_isFirstBallCollisionInTurn)
        {
            return;
        }

        if (ballABallType != BallType.White && ballBBallType != BallType.White)
        {
            return;
        }

        Ball nonWhiteBall = msg.BallA;
        if (ballABallType == BallType.White)
        {
            nonWhiteBall = msg.BallB;
        }

        Debug.Log($"First ball contact {ballABallType} - {ballBBallType}");

        if (!_gameRules.IsLegalFirstContactWithWhiteBall(nonWhiteBall.BallType))
        {
            _foulText.AnimateTextIn();
            _gameRules.CurrentPlayerTouchedIllegalBallFirst(nonWhiteBall);
        }
        
        _isFirstBallCollisionInTurn = false;
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
        _gameSetup.End();
        
        Messenger.RemoveListener<BallStartedMoving>(OnBallStartedMoving);
        Messenger.RemoveListener<BallStoppedMoving>(OnBallStoppedMoving);
        Messenger.RemoveListener<PlayerAnnouncedShot>(OnPlayerAnnouncedShot);
        Messenger.RemoveListener<BallCollidedWithBall>(OnBallCollidedWithBall);

        _cameraDirector.End();
        _soundManager.End();
    }

    public void ResetGame()
    {
        EndGame();
        StartNewGame();
        _whiteBall.Respot();
        _cue.StartNewTurn();
        _cameraDirector.StartNewTurn(_gameRules.NextBallOnPosition);
    }
}
