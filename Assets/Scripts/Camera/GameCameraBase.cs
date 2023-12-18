using UnityEngine;

public abstract class GameCameraBase : MonoBehaviour
{
    [SerializeField] private CameraType _cameraType;

    public CameraType CameraType => _cameraType;

    public abstract void Init();

    public abstract void Activate();

    public abstract void Deactivate();
}