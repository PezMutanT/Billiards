using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraDirector : MonoBehaviour
{
    [SerializeField] private List<GameCamera> _gameCameras;
    [SerializeField] private RenderTexture _renderTexture;
    [SerializeField] private GameObject _renderTextureGameObject;
    [SerializeField] private List<Transform> _holes;
    
    private Dictionary<CameraType, GameCamera> _camerasData;
    private GameCamera _currentCamera;
    private GameCamera _activeSecondaryCamera;

    public void Init()
    {
        _camerasData = new Dictionary<CameraType, GameCamera>();
        foreach (var camera in _gameCameras)
        {
            camera.Init();
            camera.Deactivate();
            _camerasData.Add(camera.CameraType, camera);
        }

        _currentCamera = _camerasData[CameraType.PLAYER];
        _currentCamera.Activate();
        _activeSecondaryCamera = null;
    }

    public void ActivateCamera(CameraType cameraType)
    {
        _currentCamera.Deactivate();
        _currentCamera = _camerasData[cameraType];
        _currentCamera.RenderFullScreen();
        _currentCamera.Activate();
    }

    public void ToggleSecondaryCamera(CameraType cameraType)
    {
        var newSecondaryCamera  = _camerasData[cameraType];
        
        if (_activeSecondaryCamera != null)
        {
            _activeSecondaryCamera.Deactivate();
        }

        if (_activeSecondaryCamera == newSecondaryCamera)
        {
            _renderTextureGameObject.SetActive(false);
            _activeSecondaryCamera.Deactivate();
            _activeSecondaryCamera = null;
        }
        else
        {
            _renderTextureGameObject.SetActive(true);
            newSecondaryCamera.RenderInCornerOfScreen(_renderTexture);
            _activeSecondaryCamera = newSecondaryCamera;
            _activeSecondaryCamera.Activate();
        }
    }

    public void StartNewTurn()
    {
        ActivateCamera(CameraType.PLAYER);
    }

    public void End()
    {
        _camerasData.Clear();
    }

    public void SwitchToShotCamera(Transform cueBall, Vector3 direction)
    {
        var ballRadius = cueBall.gameObject.GetComponent<SphereCollider>().radius;
        if (Physics.SphereCast(
                cueBall.position,
                ballRadius, direction,
                out var hit,
                LayerMask.GetMask("RaycastBalls")))
        {
            var targetBall = hit.collider.transform;
            Debug.Log($"First potential collision with {targetBall.gameObject.name}");

            var shotCamera = DetermineShotCamera(cueBall, targetBall);
            ActivateCamera(shotCamera);
        }
    }
    
    private CameraType DetermineShotCamera(Transform cueBall, Transform targetBall)
    {
        Vector3 direction = (targetBall.position - cueBall.position).normalized;

        float maxDotProduct = float.MinValue;
        int likelyPottingHoleIndex = -1;

        for (int i = 0; i < _holes.Count; i++)
        {
            Vector3 toHole = (_holes[i].position - cueBall.position).normalized;
            float dotProduct = Vector3.Dot(direction, toHole);

            if (dotProduct > maxDotProduct)
            {
                maxDotProduct = dotProduct;
                likelyPottingHoleIndex = i;
            }
        }

        return GetCameraTypeForHoleIndex(likelyPottingHoleIndex);
    }

    private CameraType GetCameraTypeForHoleIndex(int holeIndex)
    {
        switch (holeIndex)
        {
            case 0: return CameraType.TOP_LEFT_POT;
            case 1: return CameraType.TOP_RIGHT_POT;
            case 2: return CameraType.BOTTOM_LEFT_POT;
            case 3: return CameraType.BOTTOM_RIGHT_POT;
            case 4: return CameraType.SIDE_LEFT_POT;
            case 5: return CameraType.SIDE_RIGHT_POT;
                
            default:
                Debug.LogWarning($"Hole index {holeIndex} not found.");
                return CameraType.TV;
        }
    }
}