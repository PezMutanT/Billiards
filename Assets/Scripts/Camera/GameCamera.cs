using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GameCamera : MonoBehaviour
{
    [SerializeField] private CameraType _cameraType;

    protected Camera _camera;
    
    public CameraType CameraType => _cameraType;

    public virtual void Init()
    {
        _camera = GetComponent<Camera>();
    }

    public void Activate()
    {
        Debug.Log($"Activating camera {gameObject.name}...");
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        Debug.Log($"Deactivating camera {gameObject.name}...");
        gameObject.SetActive(false);
    }

    public void RenderFullScreen()
    {
        _camera.targetTexture = null;
    }

    public void RenderInCornerOfScreen(RenderTexture renderTexture)
    {
        _camera.targetTexture = renderTexture;
    }
}