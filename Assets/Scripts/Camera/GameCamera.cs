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
        _camera.Priority = 1;
    }

    public override void ActivateAsSecondary()
    {
        Debug.Log($"Activating secondary camera {gameObject.name}...");
        _camera.Priority = 2;
    }

    public override void Deactivate()
    {
        Debug.Log($"Deactivating camera {gameObject.name}...");
        _camera.Priority = 0;
    }
}