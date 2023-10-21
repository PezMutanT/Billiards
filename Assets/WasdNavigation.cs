using System;
using Unity.Mathematics;
using UnityEngine;

public class WasdNavigation : MonoBehaviour
{
    [SerializeField] private Transform _whiteBall;
    [SerializeField] private float _minHeight;
    [SerializeField] private float _maxHeight;

    public float moveSpeed = 5.0f; // Speed of camera movement
    public float sensitivity = 2.0f; // Mouse look sensitivity


    private float verticalRotation = 0.0f;
    private float horizontalRotation = 0.0f;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the mouse cursor to the screen
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            verticalRotation = transform.localRotation.eulerAngles.x;
            horizontalRotation = transform.localRotation.eulerAngles.y;
        }
        
        if (Input.GetMouseButton(0))
        {
            var mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            verticalRotation -= mouseInput.y * sensitivity;
            verticalRotation = Mathf.Clamp(verticalRotation, -60f, 60f);

            horizontalRotation += mouseInput.x * sensitivity;

            transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            if (horizontalInput == 0f && verticalInput == 0f)
            {
                return;
            }

            Vector3 moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;

            var newPosition = transform.position + (moveDirection * moveSpeed * Time.deltaTime);
            newPosition.y = Mathf.Clamp(newPosition.y, _minHeight, _maxHeight);

            transform.position = newPosition;
        }
    }
}