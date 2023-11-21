using System.Collections.Generic;
using Messaging;
using UnityEngine;

public class GameRules
{
    private Player _player1;
    private Player _player2;
    private Player _currentPlayer;
    private List<Ball> _ballsInPlay;
    private List<Ball> _ballsPottedThisTurn;
    private List<Ball> _ballsPottedPreviousTurn;
    private BallOnDecider _ballOnDecider;
    private bool _hasToChangePlayerAtEndOfTurn;

    public Vector3 NextBallOnPosition => _ballOnDecider.NextBallOnPosition();
    
    public void Init(List<Ball> allBalls)
    {
        Messenger.AddListener<BallCollidedWithBall>(OnBallCollidedWithBall);
        Messenger.AddListener<BallEnteredPot>(OnBallEnteredPot);

        InitPlayers();
        
        _ballsInPlay = allBalls;
        _ballsPottedThisTurn = new List<Ball>();
        _ballsPottedPreviousTurn = new List<Ball>();
        _ballOnDecider = new BallOnDecider(_ballsInPlay);
        _hasToChangePlayerAtEndOfTurn = false;
    }

    private void InitPlayers()
    {
        _player1 = new Player(1);
        _player1.Init();

        _player2 = new Player(2);
        _player2.Init();

        _currentPlayer = _player1;
    }

    public void StartNewTurn()
    {
        if (!_hasToChangePlayerAtEndOfTurn)
        {
            _ballsPottedPreviousTurn = new List<Ball>(_ballsPottedThisTurn);
            _ballsPottedThisTurn.Clear();
            return;
        }
        
        _ballsPottedPreviousTurn.Clear();
        _ballsPottedThisTurn.Clear();

        if (_currentPlayer == _player1)
        {
            _currentPlayer = _player2;
        }
        else
        {
            _currentPlayer = _player1;
        }

        _hasToChangePlayerAtEndOfTurn = false;
    }

    public void End()
    {
        Messenger.RemoveListener<BallEnteredPot>(OnBallEnteredPot);
        Messenger.RemoveListener<BallCollidedWithBall>(OnBallCollidedWithBall);
    }

    private void OnBallCollidedWithBall(BallCollidedWithBall msg)
    {
        Debug.Log($"Ball {msg.BallA.BallType} collided with ball {msg.BallB.BallType}");
    }

    private void OnBallEnteredPot(BallEnteredPot msg)
    {
        _ballsPottedThisTurn.Add(msg.Ball);
    }

    public void CheckScoreThisTurn()
    {
        LogPottedBalls();

        if (_ballsPottedThisTurn.Count == 0)
        {
            _hasToChangePlayerAtEndOfTurn = true;
            _ballOnDecider.DetermineNextBallOnForOtherPlayer();
            return;
        }

        if (_ballsPottedThisTurn.Count > 1)
        {
            //foul
            RespotBallsIfNeeded();
            _hasToChangePlayerAtEndOfTurn = true;
            _ballOnDecider.DetermineNextBallOnForOtherPlayer();
            return;
        }

        var singleBallPotted = _ballsPottedThisTurn[0];
        if (!_ballOnDecider.IsPottedBallAllowed(singleBallPotted.BallType))
        {
            //foul
            RespotBallsIfNeeded();
            _hasToChangePlayerAtEndOfTurn = true;
            _ballOnDecider.DetermineNextBallOnForOtherPlayer();
            return;
        }
        
        _currentPlayer.AddScore(singleBallPotted.ScoreWhenPotted);
    
        _ballsInPlay.Remove(singleBallPotted);

        _ballOnDecider.DetermineNextBallOnForSamePlayer(_hasToChangePlayerAtEndOfTurn, singleBallPotted.BallType);
    }

    private void LogPottedBalls()
    {
        Debug.Log($"[GameRules:CheckScoreThisTurn]");
        var ballsPottedPreviousTurnString = "Balls potted previous turn";
        foreach (var ball in _ballsPottedPreviousTurn)
        {
            ballsPottedPreviousTurnString += $", {ball.BallType}";
        }
        Debug.Log($"{ballsPottedPreviousTurnString}");

        var ballsPottedThisTurn = "Balls potted this turn";
        foreach (var ball in _ballsPottedThisTurn)
        {
            ballsPottedThisTurn += $", {ball.BallType}";
        }
        Debug.Log($"{ballsPottedThisTurn}");
    }

    private void RespotBallsIfNeeded()
    {
        foreach (var ball in _ballsPottedThisTurn)
        {
            if (ball.BallType == BallType.White ||
                IsColorBallAfterLastRedPotted(ball))
            {
                ball.Respot();
            }
        }
    }

    private bool IsColorBallAfterLastRedPotted(Ball ball)
    {
        if (ball.BallType == BallType.Red)
        {
            return false;
        }

        if (_ballsInPlay.Find(ball => ball.BallType == BallType.Red))
        {
            return true;
        }

        if (_ballsPottedPreviousTurn.Find(ball => ball.BallType == BallType.Red))
        {
            return true;
        }

        return false;
    }
}