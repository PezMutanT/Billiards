using System.Collections.Generic;
using UnityEngine;

public class BallOnDecider
{
    private readonly List<Ball> _ballsInPlay;
    private List<BallType> _ballOnAllowedTypes;
    private static readonly List<BallType> _allColors = new List<BallType>()
    {
        BallType.Yellow,
        BallType.Green,
        BallType.Brown,
        BallType.Blue,
        BallType.Pink,
        BallType.Black
    };

    public BallOnDecider(List<Ball> ballsInPlay)
    {
        _ballsInPlay = ballsInPlay;
        _ballOnAllowedTypes = new List<BallType>();
    }

    public bool IsPottedBallAllowed(BallType pottedBallType)
    {
        return _ballOnAllowedTypes.Contains(pottedBallType);
    }

    public void DetermineNextBallOnForSamePlayer(bool hasToChangePlayerAtEndOfTurn, BallType pottedBallType)
    {
        _ballOnAllowedTypes.Clear();
        
        if (!hasToChangePlayerAtEndOfTurn && pottedBallType == BallType.Red)
        {
            Debug.Log("Next ball on: all colors");
            _ballOnAllowedTypes = new List<BallType>(_allColors);
            return;
        }

        SetAllowedBallTypeToNextBallInPlay();
    }

    public void DetermineNextBallOnForOtherPlayer()
    {
        _ballOnAllowedTypes.Clear();
        
        SetAllowedBallTypeToNextBallInPlay();
    }

    private void SetAllowedBallTypeToNextBallInPlay()
    {
        var nextBallOnType = _ballsInPlay[^1].BallType;
        Debug.Log($"Next ball on: {nextBallOnType}");
        _ballOnAllowedTypes.Add(nextBallOnType);
    }

    public Vector3 NextBallOnPosition()
    {
        if (_ballOnAllowedTypes.Count == 0)
        {
            Debug.LogError($"Allowed ball types list is empty.");
        }
        
        // if (_ballOnAllowedTypes.Count == 1)
        {
            var foundBall = _ballsInPlay.Find(ball => ball.BallType == _ballOnAllowedTypes[0]);
            if (foundBall == null)
            {
                Debug.LogError($"Next ball {_ballOnAllowedTypes[0]} not found.");
                return Vector3.zero;
            }

            return foundBall.transform.position;
        }
    }
}