using Messaging;
using UnityEngine;

public class GameRules
{
    private Player _player1;
    private Player _player2;
    private Player _currentPlayer;
    
    public void Init()
    {
        Messenger.AddListener<BallCollidedWithBall>(OnBallCollidedWithBall);
        Messenger.AddListener<BallEnteredPot>(OnBallEnteredPot);

        InitPlayers();
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
        //TODO - rule check if legal pot
        
        _currentPlayer.AddScore(msg.Ball.ScoreWhenPotted);
    }
}