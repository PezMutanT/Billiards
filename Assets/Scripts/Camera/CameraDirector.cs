using System.Collections.Generic;
using UnityEngine;

public class CameraDirector : MonoBehaviour
{
    [SerializeField] private List<GameCamera> _gameCameras;
    [SerializeField] private RenderTexture _renderTexture;
    [SerializeField] private GameObject _renderTextureGameObject;
    
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
}