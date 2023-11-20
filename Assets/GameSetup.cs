using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    [SerializeField] protected GameObject _blackBallPrefab;
    [SerializeField] protected GameObject _pinkBallPrefab;
    [SerializeField] protected GameObject _blueBallPrefab;
    [SerializeField] protected GameObject _brownBallPrefab;
    [SerializeField] protected GameObject _greenBallPrefab;
    [SerializeField] protected GameObject _yellowBallPrefab;
    [SerializeField] protected GameObject _redBallPrefab;

    public List<Ball> AllBalls => _allBalls;
    
    private List<Ball> _allBalls;

    private const float _tableLength = 35.69f;
    //private const float _tableWidth = 17.78f;
    private const float _ballSize = 0.525f;
    private const float _ballOffsetConstant = 0.8661f;

    public void Init()
    {
        SetupBalls();
        InitBalls();
    }
    
    private void SetupBalls()
    {
        _allBalls = new List<Ball>();
        
        var halfTableLength = _tableLength * 0.5f;

        var blackSpotX = -halfTableLength + MetersToUnityUnits(0.324f);
        var pinkSpotX = blackSpotX * 0.5f;
        var blueSpotX = 0.0f;
        var brownSpotX = halfTableLength - MetersToUnityUnits(0.737f);
        var yellowSpotY = MetersToUnityUnits(0.292f);
        var greenSpotY = -yellowSpotY;
        var firstRedSpotX = pinkSpotX - _ballSize;

        foreach (var ball in _allBalls)
        {
            Destroy(ball);
        }
        _allBalls.Clear();
        
        _allBalls.Add(Instantiate(_blackBallPrefab, new Vector3(blackSpotX, 0f, 0f), Quaternion.identity).GetComponent<Ball>());
        _allBalls.Add(Instantiate(_pinkBallPrefab, new Vector3(pinkSpotX, 0f, 0f), Quaternion.identity).GetComponent<Ball>());
        _allBalls.Add(Instantiate(_blueBallPrefab, new Vector3(blueSpotX, 0f, 0f), Quaternion.identity).GetComponent<Ball>());
        _allBalls.Add(Instantiate(_brownBallPrefab, new Vector3(brownSpotX, 0f, 0f), Quaternion.identity).GetComponent<Ball>());
        _allBalls.Add(Instantiate(_greenBallPrefab, new Vector3(brownSpotX, 0f, greenSpotY), Quaternion.identity).GetComponent<Ball>());
        _allBalls.Add(Instantiate(_yellowBallPrefab, new Vector3(brownSpotX, 0f, yellowSpotY), Quaternion.identity).GetComponent<Ball>());

        SetupRedBalls(firstRedSpotX);
    }

    private void SetupRedBalls(float firstRedSpotX)
    {
        var redBallNameIndex = 1;
        for (var i = 0; i < 5; i++)
        {
            for (var j = 0; j < (i + 1); j++)
            {
                var newBallPosition = new Vector3(
                    firstRedSpotX - (_ballOffsetConstant * i * _ballSize),
                    0f,
                    (-0.5f * i * _ballSize) + j * _ballSize);

                var redBall = Instantiate(_redBallPrefab, newBallPosition, Quaternion.identity).GetComponent<Ball>();
                redBall.name = $"Red {redBallNameIndex}";
                redBallNameIndex++;
                _allBalls.Add(redBall);
            }
        }
    }
    
    private void InitBalls()
    {
        foreach (var ball in _allBalls)
        {
            ball.Init();
        }
    }

    private float MetersToUnityUnits(float meters)
    {
        return meters * 10f;
    }
}