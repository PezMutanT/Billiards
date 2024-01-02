using UnityEngine;

[CreateAssetMenu(menuName = "Globals")]
public class GlobalConfiguration : ScriptableObject
{
    public float BallMinVelocityThreshold;
    public float BallsSleepThreshold;
    public float ForceChargeVelocity;
    public float MinCueForceMagnitude;
    public float MaxCueForceMagnitude;
    public float ShootDelaySeconds;
    public float CameraInputSensitivityLow;
    public float CameraInputSensitivityHigh;
}
