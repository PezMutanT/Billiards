using UnityEngine;

[RequireComponent(typeof(CameraDirector))]
public class CameraToggler : MonoBehaviour
{
    private CameraDirector _cameraDirector;
    
    private void Awake()
    {
        _cameraDirector = GetComponent<CameraDirector>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            _cameraDirector.ActivateCamera(CameraType.PLAYER);
        }
        
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            _cameraDirector.ActivateCamera(CameraType.BOTTOM);
        }
        
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            _cameraDirector.ActivateCamera(CameraType.TOP_FULL_TABLE);
        }
    }
}