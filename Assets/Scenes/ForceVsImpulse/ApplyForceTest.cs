using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForceTest : MonoBehaviour
{
    [SerializeField] private Vector3 _force;
    private Rigidbody _target;

    private enum State
    {
        ONLY_UPDATE,
        ONLY_FIXED_UPDATE,
        INPUT_IN_UPDATE_FORCE_IN_FIXED_UPDATE
    }

    private State _state;
    private bool _hasTriggeredInput = false;

    void Start()
    {
        _target = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            int stateInt = (int)_state;
            stateInt = (stateInt + 1) % 3;
            _state = (State)stateInt;
            Debug.Log($"State: {_state.ToString()}");
        }

        if (_state == State.ONLY_FIXED_UPDATE)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_state == State.INPUT_IN_UPDATE_FORCE_IN_FIXED_UPDATE)
            {
                _hasTriggeredInput = true;
            }

            _target.AddForce(_force);
        }
    }
    
    void FixedUpdate()
    {
        if (_state == State.ONLY_UPDATE)
        {
            return;
        }
        
        if (_state == State.ONLY_FIXED_UPDATE && Input.GetKeyDown(KeyCode.F))
        {
            _target.AddForce(_force);
        }

        if (_state == State.INPUT_IN_UPDATE_FORCE_IN_FIXED_UPDATE && _hasTriggeredInput)
        {
            _hasTriggeredInput = false;
            _target.AddForce(_force);
        }
    }
}
