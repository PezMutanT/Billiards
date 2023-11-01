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
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _forceMagnitude = _globalConfiguration.MaxCueForceMagnitude;
            _cue.DebugShoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _forceMagnitude = _globalConfiguration.MaxCueForceMagnitude * 0.1f;
            _cue.DebugShoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _forceMagnitude = _globalConfiguration.MaxCueForceMagnitude * 0.2f;
            _cue.DebugShoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _forceMagnitude = _globalConfiguration.MaxCueForceMagnitude * 0.3f;
            _cue.DebugShoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _forceMagnitude = _globalConfiguration.MaxCueForceMagnitude * 0.4f;
            _cue.DebugShoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _forceMagnitude = _globalConfiguration.MaxCueForceMagnitude * 0.5f;
            _cue.DebugShoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            _forceMagnitude = _globalConfiguration.MaxCueForceMagnitude * 0.6f;
            _cue.DebugShoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            _forceMagnitude = _globalConfiguration.MaxCueForceMagnitude * 0.7f;
            _cue.DebugShoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            _forceMagnitude = _globalConfiguration.MaxCueForceMagnitude * 0.8f;
            _cue.DebugShoot(_forceMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            _forceMagnitude = _globalConfiguration.MaxCueForceMagnitude * 0.9f;
            _cue.DebugShoot(_forceMagnitude);
        }
    }
}
