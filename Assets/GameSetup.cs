using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    [SerializeField] private Transform _table;
    [SerializeField] private GameObject _blackBallPrefab;
    [SerializeField] private GameObject _pinkBallPrefab;
    [SerializeField] private GameObject _blueBallPrefab;
    [SerializeField] private GameObject _brownBallPrefab;
    [SerializeField] private GameObject _greenBallPrefab;
    [SerializeField] private GameObject _yellowBallPrefab;
    [SerializeField] private GameObject _redBallPrefab;

    private List<GameObject> _allBalls = new List<GameObject>();
    
    private const float _ballSize = 52.5f;
    private const float _ballOffsetConstant = 0.8661f;   //

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        SetupBalls();
    }

    public void SetupBalls()
    {
        var tableWidth = _table.localScale.x;
        var halfTableWidth = tableWidth * 0.5f;

        var blackSpotX = -halfTableWidth + MmToUnityUnits(324.0f);
        var pinkSpotX = blackSpotX * 0.5f;
        var blueSpotX = 0.0f;
        var brownSpotX = halfTableWidth - MmToUnityUnits(737.0f);
        var yellowSpotY = MmToUnityUnits(292.0f);
        var greenSpotY = -yellowSpotY;
        var firstRedSpotX = pinkSpotX - 1.0f;

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
                    firstRedSpotX - (_ballOffsetConstant * i),
                    0f,
                    (-0.5f * i) + j);

                _allBalls.Add(Instantiate(_redBallPrefab, newBallPosition, Quaternion.identity));
            }
        }
    }

    private float MmToUnityUnits(float millimeters)
    {
        return millimeters / _ballSize;
    }
}
