using System.Collections.Generic;
using Messaging;
using UnityEngine;

public class GameRules
{
    private Player _player1;
    private Player _player2;
    private Player _currentPlayer;
    private BallType _ballOn;
    private List<Ball> _ballsPottedThisTurn;
    private bool _hasToChangePlayerAtEndOfTurn;
    
    public void Init()
    {
        Messenger.AddListener<BallCollidedWithBall>(OnBallCollidedWithBall);
        Messenger.AddListener<BallEnteredPot>(OnBallEnteredPot);

        InitPlayers();
        
        _ballOn = BallType.Red;
        _ballsPottedThisTurn = new List<Ball>();
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
        _ballsPottedThisTurn.Clear();
        
        if (!_hasToChangePlayerAtEndOfTurn)
        {
            return;
        }
        
        if (_currentPlayer == _player1)
        {
            _currentPlayer = _player2;
        }
        else
        {
            _currentPlayer = _player1;
        }
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
        if (_ballsPottedThisTurn.Count == 0)
        {
            _hasToChangePlayerAtEndOfTurn = true;
            return;
        }

        if (_ballsPottedThisTurn.Count > 1)
        {
            //foul
            //respot balls if needed
            _hasToChangePlayerAtEndOfTurn = true;
            return;
        }

        var singleBallPotted = _ballsPottedThisTurn[0];
        if (singleBallPotted.BallType != _ballOn)
        {
            //foul
            //respot balls if needed
            _hasToChangePlayerAtEndOfTurn = true;
            return;
        }
        
        _currentPlayer.AddScore(singleBallPotted.ScoreWhenPotted);
    }
}