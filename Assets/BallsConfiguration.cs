using UnityEngine;

[CreateAssetMenu(menuName = "Balls configuration")]
public class BallsConfiguration : ScriptableObject
{
    public Texture[] Balls;
    public float HitStrength;
}
