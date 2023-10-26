using System;
using System.Collections.Generic;
using UnityEngine;

namespace Messaging
{
    public class Message
    {
    }

    public class BallCollidedWithBall : Message
    {
        public readonly Ball BallA;
        public readonly Ball BallB;
        
        public BallCollidedWithBall(Ball ballA, Ball ballB)
        {
            BallA = ballA;
            BallB = ballB;
        }
    }

    public class BallEnteredPot : Message
    {
        public readonly Ball Ball;
        public readonly Pot Pot;
        
        public BallEnteredPot(Ball ball, Pot pot)
        {
            Ball = ball;
            Pot = pot;
        }
    }

    public class PlayerScoreChanged : Message
    {
        public Player Player;
        public int OldScore;
        public int NewScore;

        public PlayerScoreChanged(Player player, int oldScore, int newScore)
        {
            Player = player;
            OldScore = oldScore;
            NewScore = newScore;
        }
    }

    public class BallStartedMoving : Message
    {
        public Ball Ball;

        public BallStartedMoving(Ball ball)
        {
            Ball = ball;
        }
    }

    public class BallStoppedMoving : Message
    {
        public Ball Ball;

        public BallStoppedMoving(Ball ball)
        {
            Ball = ball;
        }
    }
    
    public static class Messenger
    {
        private static readonly Dictionary<Type, List<Delegate>> _listenersByMessage = new Dictionary<Type, List<Delegate>>();
        
        public static void AddListener<T>(Action<T> callback) where T : Message
        {
            var tryGetValue = _listenersByMessage.TryGetValue(typeof(T), out var existingMessageCallbacks);
            var existsMessage = tryGetValue;
            if (existsMessage)
            {
                existingMessageCallbacks.Add(callback);
            }
            else
            {
                _listenersByMessage.Add(typeof(T), new List<Delegate>() { callback });
            }
        }

        public static void RemoveListener<T>(Action<T> callback) where T : Message
        {
            var existsMessage = _listenersByMessage.TryGetValue(typeof(T), out var existingMessageCallbacks);
            if (existsMessage)
            {
                var foundCallback = existingMessageCallbacks.Find(x => x == callback);
                existingMessageCallbacks.Remove(foundCallback);
            }
        }
        
        public static void Send<T>(T message) where T : Message
        {
            var existsMessage = _listenersByMessage.TryGetValue(typeof(T), out var existingMessageCallbacks);
            if (existsMessage)
            {
                foreach (var listener in existingMessageCallbacks)
                {
                    ((Action<T>)listener).Invoke(message);
                }
            }
        }
    }
}