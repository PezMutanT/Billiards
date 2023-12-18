using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitAroundTarget : GameCamera
{
    [SerializeField] private Transform _target1;
    [SerializeField] private Transform _target2;
    [SerializeField] private Cue _cue;
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private float _distanceToTarget;
    [SerializeField] private float _height;
    [SerializeField] private float _maxAngle;
    [SerializeField] private LayerMask _targetObjectsLayerMask;
    [SerializeField] private float _translationTimeSeconds;

    private Vector2 _deltaMouseInput;
    private float _elapsedTime;
    private bool _isMoving;
    private Vector3 _originalPosition;
    private Vector3 _targetPosition;

    public /*override*/ void Init()
    {
        base.Init();
        
        _deltaMouseInput = Vector2.zero;
        _elapsedTime = 0f;
        _isMoving = false;
        _originalPosition = transform.position;
        
        var target2Position = _target2 == null ? Vector3.zero : _target2.position;
        SetPositionLookingAtBothTargets(target2Position);
    }

    public void SetPositionLookingAtBothTargets(Vector3 target2Position)
    {
        _isMoving = true;
        _originalPosition = transform.position;
        _elapsedTime = 0f;

        var direction = (target2Position - _target1.position).normalized;
        var newPosition = _target1.position - (direction * _distanceToTarget);
        newPosition.y = _height;

        _targetPosition = newPosition;
    }

    private void Update()
    {
        if (_isMoving)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            _deltaMouseInput.x = Input.GetAxis("Mouse X");
            _deltaMouseInput.y = -Input.GetAxis("Mouse Y");
        }

        if (Input.GetMouseButtonUp(0))
        {
            _deltaMouseInput.x = 0f;
            _deltaMouseInput.y = 0f;
            
            /*Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 100f, _targetObjectsLayerMask))
            {
                Renderer renderer = hit.collider.GetComponent<Renderer>();
                if (renderer != null)
                {
                    SetPositionLookingAtBothTargets(hit.transform.position);
                }
            }*/
        }
    }

    void LateUpdate()
    {
        if (_isMoving)
        {
            var translationInterpolator = Mathf.Clamp(_elapsedTime / _translationTimeSeconds, 0f, 1f);
            if (translationInterpolator == 1f)
            {
                _isMoving = false;
            }
            
            transform.position = Vector3.Lerp(_originalPosition, _targetPosition, translationInterpolator);
            _elapsedTime += Time.deltaTime;
        }

        var previousPosition = transform.position;
        
        transform.RotateAround(_target1.position, Vector3.up, _deltaMouseInput.x * _mouseSensitivity * Time.deltaTime);
        transform.RotateAround(_target1.position, transform.right, _deltaMouseInput.y * _mouseSensitivity * Time.deltaTime);

        if (transform.rotation.eulerAngles.x > _maxAngle)
        {
            transform.position = previousPosition;
        }
        
        LookToTarget1();
        
        _cue.UpdateFromCamera(transform.forward);
    }

    private void LookToTarget1()
    {
        var forwardDirection = _target1.position - transform.position;
        var newRotation = Quaternion.LookRotation(forwardDirection);

        transform.rotation = newRotation;
    }
}
