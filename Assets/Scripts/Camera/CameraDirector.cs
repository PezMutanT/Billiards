using System.Collections.Generic;
using UnityEngine;

public class CameraDirector : MonoBehaviour
{
    [SerializeField] private List<GameCamera> _gameCameras;
    
    private Dictionary<CameraType, GameCamera> _camerasData;
    private GameCamera _currentCamera;

    public void Init()
    {
        _camerasData = new Dictionary<CameraType, GameCamera>();
        foreach (var camera in _gameCameras)
        {
            camera.Deactivate();
            _camerasData.Add(camera.CameraType, camera);
        }

        _currentCamera = _camerasData[CameraType.PLAYER];
        _currentCamera.Activate();
    }

    public void ActivateCamera(CameraType cameraType)
    {
        _currentCamera.Deactivate();
        _currentCamera = _camerasData[cameraType];
        _currentCamera.Activate();
    }

    public void End()
    {
        _camerasData.Clear();
    }
}