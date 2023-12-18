using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class PlayerCamera : GameCameraBase
{
    [SerializeField] private Transform _mainCamera;
    [SerializeField] private Cue _cue;

    private CinemachineFreeLook _camera;
    private CinemachineCore.AxisInputDelegate _originalInputDelegate;

    public override void Init()
    {
        _camera = GetComponent<CinemachineFreeLook>();
        _originalInputDelegate = CinemachineCore.GetInputAxis;
        DisableCameraMoving();
    }

    public override void Activate()
    {
        Debug.Log($"Activating camera {gameObject.name}...");
        _camera.Priority = 1;
    }

    public override void Deactivate()
    {
        Debug.Log($"Deactivating camera {gameObject.name}...");
        _camera.Priority = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            EnableCameraMoving();
        }

        if (Input.GetMouseButtonUp(0))
        {
            DisableCameraMoving();
        }
    }

    private void EnableCameraMoving()
    {
        CinemachineCore.GetInputAxis = _originalInputDelegate;
    }

    private void DisableCameraMoving()
    {
        CinemachineCore.GetInputAxis = (axisName) => 0;
    }

    private void LateUpdate()
    {
        _cue.UpdateFromCamera(_mainCamera.forward);
    }
}