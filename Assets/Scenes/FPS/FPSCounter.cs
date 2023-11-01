using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private float _elapsedTime = 0f;
    private int _frameCount = 0;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _frameCount++;
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= 1f)
        {
            
            
            _elapsedTime = 0f;
            _frameCount = 0;
        }
    }
}
