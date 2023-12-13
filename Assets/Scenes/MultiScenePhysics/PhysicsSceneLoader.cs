using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiScenePhysicsMain : MonoBehaviour
{
    public string physicsSceneName;
    public float physicsSceneTimeScale = 1;
    private PhysicsScene physicsScene;

    [SerializeField] private LineRenderer _lineRenderer;

    private void Start()
    {
        LoadSceneParameters param = new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.Physics3D);
        Scene scene = SceneManager.LoadScene(physicsSceneName, param);
        physicsScene = scene.GetPhysicsScene();
    }

    void FixedUpdate()
    {
        if (physicsScene != null)
        {
            /*physicsScene.Simulate(Time.fixedDeltaTime * physicsSceneTimeScale);
            _lineRenderer.SetPosition();*/
        }
    }
}
