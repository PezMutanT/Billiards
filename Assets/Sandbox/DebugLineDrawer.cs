using System.Collections.Generic;
using UnityEngine;

public class DebugLineDrawer : MonoBehaviour
{
    [SerializeField] private Transform _target1;
    [SerializeField] private Transform _target2;

    private readonly List<DebugVectorInfo> _debugVectors = new List<DebugVectorInfo>();

    private Vector3 _cameraToTarget1Direction;
    private DebugVectorInfo _cameraToTarget1DirectionDebug;
    
    private Vector3 _target1ToTarget2Direction;
    private DebugVectorInfo _target1ToTarget2DirectionDebug;

    private void OnDrawGizmos()
    {
        foreach (var debugVector in _debugVectors)
        {
            Gizmos.color = debugVector.Color;
            Gizmos.DrawLine(debugVector.From, debugVector.To);
        }
    }

    void Start()
    {
        _cameraToTarget1DirectionDebug = new DebugVectorInfo(Color.red, transform.position, _target1.position); 
        _target1ToTarget2DirectionDebug = new DebugVectorInfo(Color.blue, _target1.position, _target2.position); 
        
        _debugVectors.Add(_cameraToTarget1DirectionDebug);
        _debugVectors.Add(_target1ToTarget2DirectionDebug);
    }

    void Update()
    {
        _cameraToTarget1DirectionDebug.UpdateLine(_target1.position, transform.position);
        _target1ToTarget2DirectionDebug.UpdateLine(_target1.position, _target2.position);
    }
}

public class DebugVectorInfo
{
    public Color Color;
    public Vector3 From;
    public Vector3 To;

    public DebugVectorInfo(Color color, Vector3 from, Vector3 to)
    {
        Color = color;
        From = from;
        To = to;
    }

    public void UpdateLine(Vector3 from, Vector3 to)
    {
        From = from;
        To = to;
    }
}
