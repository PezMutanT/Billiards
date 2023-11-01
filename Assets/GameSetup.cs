using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    [SerializeField] private MeshRenderer _tableMesh;
    [SerializeField] protected GameObject _blackBallPrefab;
    [SerializeField] protected GameObject _pinkBallPrefab;
    [SerializeField] protected GameObject _blueBallPrefab;
    [SerializeField] protected GameObject _brownBallPrefab;
    [SerializeField] protected GameObject _greenBallPrefab;
    [SerializeField] protected GameObject _yellowBallPrefab;
    [SerializeField] protected GameObject _redBallPrefab;

    private List<GameObject> _allBalls = new List<GameObject>();
    
    private const float _ballSize = 0.525f;
    private const float _ballOffsetConstant = 0.8661f;

    public void Init()
    {
        SetupBalls();
        InitBalls();
    }
    
    private void SetupBalls()
    {
        var meshScale = _tableMesh.transform.localScale;
        var tableLength = _tableMesh.bounds.size.x;
        var halfTableLength = tableLength * 0.5f;

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
        
        _allBalls.Add(Instantiate(_blackBallPrefab, new Vector3(blackSpotX, 0f, 0f), Quaternion.identity));
        _allBalls.Add(Instantiate(_pinkBallPrefab, new Vector3(pinkSpotX, 0f, 0f), Quaternion.identity));
        _allBalls.Add(Instantiate(_blueBallPrefab, new Vector3(blueSpotX, 0f, 0f), Quaternion.identity));
        _allBalls.Add(Instantiate(_brownBallPrefab, new Vector3(brownSpotX, 0f, 0f), Quaternion.identity));
        _allBalls.Add(Instantiate(_greenBallPrefab, new Vector3(brownSpotX, 0f, greenSpotY), Quaternion.identity));
        _allBalls.Add(Instantiate(_yellowBallPrefab, new Vector3(brownSpotX, 0f, yellowSpotY), Quaternion.identity));

        SetupRedBalls(firstRedSpotX);
    }

    private void SetupRedBalls(float firstRedSpotX)
    {
        for (var i = 0; i < 5; i++)
        {
            for (var j = 0; j < (i + 1); j++)
            {
                var newBallPosition = new Vector3(
                    firstRedSpotX - (_ballOffsetConstant * i * _ballSize),
                    0f,
                    (-0.5f * i * _ballSize) + j * _ballSize);

                _allBalls.Add(Instantiate(_redBallPrefab, newBallPosition, Quaternion.identity));
            }
        }
    }
    
    private void InitBalls()
    {
        foreach (var ballGameObject in _allBalls)
        {
            Ball ball = ballGameObject.GetComponent<Ball>();
            if (ball == null)
            {
                Debug.LogError($"Error loading ball {ballGameObject.name}");
                return;
            }
            
            ball.Init();
        }
    }

    private float MetersToUnityUnits(float meters)
    {
        return meters * 10f;
    }
}