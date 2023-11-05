using System;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private CameraType _cameraType;

    public CameraType CameraType => _cameraType;

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
}