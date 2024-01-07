using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShootProjection : MonoBehaviour
{
    [SerializeField] private List<Transform> _simulationObjects;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private int _maxSimulationIterations;
    [SerializeField] private float _velocity;

    private Scene _simulationScene;
    private PhysicsScene _physicsScene;
    private Rigidbody _ghostWhiteBall;
    private List<Transform> _ballsInPlay;

    public void Init(Ball whiteBall, List<Ball> ballsInPlay)
    {
        _ballsInPlay = new List<Transform>();
        CreatePhysicsScene();
        CreateObjectForSimulateTrajectory(whiteBall, whiteBall.transform.position);
        
        StartNewTurn(ballsInPlay);
    }

    private void CreatePhysicsScene()
    {
        _simulationScene =
            SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        _physicsScene = _simulationScene.GetPhysicsScene();

        foreach (var simulationObject in _simulationObjects)
        {
            var ghostObj = Instantiate(
                simulationObject.gameObject,
                simulationObject.position,
                simulationObject.rotation);

            var renderers = simulationObject.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                renderer.enabled = false;
            }

            SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
        }
    }

    private void CreateObjectForSimulateTrajectory(Ball ball, Vector3 whiteBallPosition)
    {
        var whiteBallGameObject = Instantiate(ball, whiteBallPosition, Quaternion.identity);
        _ghostWhiteBall = whiteBallGameObject.GetComponent<Rigidbody>();
        whiteBallGameObject.GetComponent<Renderer>().enabled = false;
        SceneManager.MoveGameObjectToScene(_ghostWhiteBall.gameObject, _simulationScene);
    }

    public void StartNewTurn(List<Ball> ballsInPlay)
    {
        _ballsInPlay.Clear();
        foreach (var ball in ballsInPlay)
        {
            var ballGameObject = Instantiate(ball, ball.transform.position, Quaternion.identity);
            ball.Init(true);
            ballGameObject.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ballGameObject.gameObject, _simulationScene);
            
            _ballsInPlay.Add(ball.transform);
        }
    }

    public void SimulateTrajectory(Vector3 whiteBallPosition, Vector3 direction)
    {
        _ghostWhiteBall.velocity = Vector3.zero;
        _ghostWhiteBall.angularVelocity = Vector3.zero;
        _ghostWhiteBall.transform.position = whiteBallPosition;
        
        _ghostWhiteBall.AddForce(direction * _velocity, ForceMode.Impulse);
        
        
        _lineRenderer.positionCount = _maxSimulationIterations;
        for (var i = 0; i < _maxSimulationIterations; i++)
        {
            _physicsScene.Simulate(Time.fixedDeltaTime);
            _lineRenderer.SetPosition(i, _ghostWhiteBall.transform.position);
        }
        
        // Destroy(ghostWhiteBall);
    }
}
