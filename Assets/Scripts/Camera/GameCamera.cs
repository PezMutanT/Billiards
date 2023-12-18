using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class GameCamera : GameCameraBase
{
    private CinemachineVirtualCamera _camera;

    public override void Init()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
    }

    public override void Activate()
    {
        Debug.Log($"Activating camera {gameObject.name}...");
        //gameObject.SetActive(true);
        _camera.Priority = 1;
    }

    public override void Deactivate()
    {
        Debug.Log($"Deactivating camera {gameObject.name}...");
        //gameObject.SetActive(false);
        _camera.Priority = 0;
    }

    public void RenderFullScreen()
    {
        //_camera.targetTexture = null;
    }

    public void RenderInCornerOfScreen(RenderTexture renderTexture)
    {
        //_camera.targetTexture = renderTexture;
    }
}