using UnityEngine;

public class CustomDebug : MonoBehaviour
{
    [SerializeField] private OrbitAroundTarget _camera;
    [SerializeField] private GameSetup _gameSetup;
    [SerializeField] private Ball _whiteBall;
    [SerializeField] private Cue _cue;
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            _camera.SetPositionLookingAtBothTargets(Vector3.zero);
            _gameSetup.SetupBalls();
            _whiteBall.DebugReset();
            // _cue.UpdateFromCamera();
        }
    }
}
