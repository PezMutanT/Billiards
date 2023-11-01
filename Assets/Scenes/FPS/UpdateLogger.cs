using UnityEngine;

public class UpdateLogger : MonoBehaviour
{
    void Update()
    {
        Debug.Log("Update");
    }
    
    void FixedUpdate()
    {
        Debug.Log("FixedUpdate");
    }
}
