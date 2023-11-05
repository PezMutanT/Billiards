using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRenderToTextureTest : MonoBehaviour
{
    [SerializeField] private Camera _camera1;
    [SerializeField] private Camera _camera2;

    [SerializeField] private RenderTexture _renderTexture;
    
    private bool _isCamera1On = true;

    void Start()
    {
        Debug.Log($"Camera.main={Camera.main}");
    }

    void Update()
    {
       if (Input.GetKeyDown(KeyCode.C))
       {
           _isCamera1On = !_isCamera1On;
           _camera1.gameObject.SetActive(_isCamera1On);
           _camera2.gameObject.SetActive(!_isCamera1On);

           if (!_isCamera1On)
           {
               _camera2.targetTexture = null;
           }
           else
           {
               _camera2.targetTexture = _renderTexture;
           }
           
           Debug.Log($"Camera.main={Camera.main}");
       }
    }
}
