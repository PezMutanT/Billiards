using UnityEngine;

public class DebugShoot : MonoBehaviour
{
    [SerializeField] private GlobalConfiguration _globalConfiguration;
    
    private Cue _cue;
    private float _forceMagnitude;

    private void Awake()
    {
        _cue = GetComponent<Cue>();
    }

    void Update()
    {
        ProcessDebugShoot();   
    }
    
    private void ProcessDebugShoot()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            _forceMagnitude = _globalConfiguration.MaxCueForceMagnitude;
            _cue.DebugShoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            _forceMagnitude = _globalConfiguration.MaxCueForceMagnitude * 0.1f;
            _cue.DebugShoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            _forceMagnitude = _globalConfiguration.MaxCueForceMagnitude * 0.2f;
            _cue.DebugShoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            _forceMagnitude = _globalConfiguration.MaxCueForceMagnitude * 0.3f;
            _cue.DebugShoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            _forceMagnitude = _globalConfiguration.MaxCueForceMagnitude * 0.4f;
            _cue.DebugShoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            _forceMagnitude = _globalConfiguration.MaxCueForceMagnitude * 0.5f;
            _cue.DebugShoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            _forceMagnitude = _globalConfiguration.MaxCueForceMagnitude * 0.6f;
            _cue.DebugShoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            _forceMagnitude = _globalConfiguration.MaxCueForceMagnitude * 0.7f;
            _cue.DebugShoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            _forceMagnitude = _globalConfiguration.MaxCueForceMagnitude * 0.8f;
            _cue.DebugShoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            _forceMagnitude = _globalConfiguration.MaxCueForceMagnitude * 0.9f;
            _cue.DebugShoot(_forceMagnitude);
        }
    }
}
