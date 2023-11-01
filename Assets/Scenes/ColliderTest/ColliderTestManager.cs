using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ColliderTestManager : MonoBehaviour
{
    [SerializeReference] private List<ResetableObject> _allResetableObjects;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (var resetableObject in _allResetableObjects)
            {
                resetableObject.DoReset();
            }
        }
    }
}
