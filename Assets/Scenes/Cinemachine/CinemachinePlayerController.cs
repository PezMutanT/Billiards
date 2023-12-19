using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachinePlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private float _sensitivity;
    
    private float horizontalInput = 0f;
    private float verticalInput = 0f;
    private CinemachineCore.AxisInputDelegate _originalInputDelegate;

    // Start is called before the first frame update
    void Start()
    {
        _originalInputDelegate = CinemachineCore.GetInputAxis;
        DisableCameraMoving();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetMouseButtonDown(0))
        {
            EnableCameraMoving();
        }

        if (Input.GetMouseButtonUp(0))
        {
            DisableCameraMoving();
        }
        
        var direccion = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        
        Vector3 movimiento = direccion * _sensitivity * Time.deltaTime;

        transform.Translate(movimiento, Space.World);
    }

    private void EnableCameraMoving()
    {
        CinemachineCore.GetInputAxis = _originalInputDelegate;
    }

    private void DisableCameraMoving()
    {
        CinemachineCore.GetInputAxis = (axisName) => 0;
    }
}
