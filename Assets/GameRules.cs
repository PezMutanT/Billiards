using System.Collections.Generic;
using System.Linq;
using Messaging;
using UnityEngine;

public class GameRules
{
    private Player _player1;
    private Player _player2;

    private Player _currentPlayer;
    private Player CurrentPlayer
    {
        get => _currentPlayer;
        set
        {
            _currentPlayer = value;
            Messenger.Send(new PlayerChanged(_currentPlayer));
        }
    }

    private Player OtherPlayer
    {
        get
        {
            if (_currentPlayer == _player1)
            {
                return _player2;
            }

            return _player1;
        }
    }
    
    private List<Ball> _ballsInPlay;
    private List<Ball> _ballsPottedThisTurn;
    private List<Ball> _ballsPottedPreviousTurn;
    private List<Ball> _respottedBallsThisTurn;
    private BallOnDecider _ballOnDecider;
    private bool _hasToChangePlayerAtEndOfTurn;
    private bool _whiteBallHasContactedAnyBall;
    private int _currentPenaltyValue;

    public List<BallType> AllowedBallTypes => _ballOnDecider.AllowedBallTypes;
    public Vector3 NextBallOnPosition => _ballOnDecider.NextBallOnPosition();
    public List<Ball> BallsInPlay => _ballsInPlay;

    public void Init(List<Ball> allBalls)
    {
        Messenger.AddListener<BallEnteredPot>(OnBallEnteredPot);
        
        _ballsInPlay = allBalls;
        _ballsPottedThisTurn = new List<Ball>();
        _ballsPottedPreviousTurn = new List<Ball>();
        _respottedBallsThisTurn = new List<Ball>();
        _ballOnDecider = new BallOnDecider(_ballsInPlay);
        _hasToChangePlayerAtEndOfTurn = false;

        InitPlayers();
    }

    private void InitPlayers()
    {
        _player1 = new Player(1);
        _player1.Init();

        _player2 = new Player(2);
        _player2.Init();

        CurrentPlayer = _player1;
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
        _respottedBallsThisTurn.Clear();

        CurrentPlayer = OtherPlayer;
        
        _hasToChangePlayerAtEndOfTurn = false;
        _whiteBallHasContactedAnyBall = false;
        _currentPenaltyValue = 0;
    }

    public void End()
    {
        Messenger.RemoveListener<BallEnteredPot>(OnBallEnteredPot);
    }

    private void OnBallEnteredPot(BallEnteredPot msg)
    {
        _ballsPottedThisTurn.Add(msg.Ball);
    }

    public void CheckScoreThisTurn()
    {
        LogPottedBalls();

        if (!_whiteBallHasContactedAnyBall)
        {
            ProcessFoul(4);
            return;
        }

        if (_ballsPottedThisTurn.Count == 0)
        {
            if (_currentPenaltyValue > 0)
            {
                ProcessFoul(_currentPenaltyValue);
                return;
            }
            
            _hasToChangePlayerAtEndOfTurn = true;
            _ballOnDecider.DetermineNextBallOnForOtherPlayer();
            return;
        }

        if (_ballsPottedThisTurn.Count > 1)
        {
            ProcessFoul(GetMaxPenaltyValue());
            return;
        }
        
        if (_currentPenaltyValue > 0)
        {
            ProcessFoul(_currentPenaltyValue);
            return;
        }

        var singleBallPotted = _ballsPottedThisTurn[0];
        if (!_ballOnDecider.IsPottedBallAllowed(singleBallPotted.BallType))
        {
            ProcessFoul(singleBallPotted.ScoreWhenPotted);
            return;
        }
        
        RespotBallsIfNeeded();

        CurrentPlayer.AddScore(singleBallPotted.ScoreWhenPotted);

        if (!_respottedBallsThisTurn.Contains(singleBallPotted))
        {
            _ballsInPlay.Remove(singleBallPotted);
        }
        
        if (IsGameOver())
        {
            Messenger.Send(new GameEnded(_player1.Score, _player2.Score));
        }
        else
        {
            _ballOnDecider.DetermineNextBallOnForSamePlayer(_hasToChangePlayerAtEndOfTurn, singleBallPotted.BallType);
        }
    }

    private int GetMaxPenaltyValue()
    {
        var maxPenaltyValueInBallsPotted = _ballsPottedThisTurn.Max(x => x.ScoreWhenPotted);
        return Mathf.Max(maxPenaltyValueInBallsPotted, _currentPenaltyValue);
    }

    private void ProcessFoul(int penaltyValue)
    {
        OtherPlayer.AddScore(penaltyValue);
        RespotBallsIfNeeded();
        _hasToChangePlayerAtEndOfTurn = true;
        _ballOnDecider.DetermineNextBallOnForOtherPlayer();
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
                HasBallToBeRespotted(ball))
            {
                _respottedBallsThisTurn.Add(ball);
                ball.Respot();
            }
        }
    }

    private bool HasBallToBeRespotted(Ball ball)
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

    public bool IsLegalFirstContactWithWhiteBall(BallType nonWhiteBallType)
    {
        _whiteBallHasContactedAnyBall = true;
        return _ballOnDecider.IsFirstBallHitAllowed(nonWhiteBallType);
    }

    public void CurrentPlayerTouchedIllegalBallFirst(Ball ball)
    {
        _currentPenaltyValue = ball.ScoreWhenPotted;
    }

    public void DebugPotNextBall()
    {
        if (_ballsInPlay == null || _ballsInPlay.Count == 0)
        {
            return;
        }

        var nextBall = _ballsInPlay[^1];
        nextBall.DebugRemoveFromGame();
        _ballsInPlay.Remove(nextBall);
        _ballOnDecider.DetermineNextBallOnForSamePlayer(false, nextBall.BallType);
    }

    public bool IsGameOver()
    {
        return _ballsInPlay.Count == 0;
    }
}