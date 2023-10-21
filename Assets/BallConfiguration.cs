using UnityEngine;

[CreateAssetMenu(menuName = "Ball configuration")]
public class BallConfiguration : ScriptableObject
{
    public BallType BallType;
    public Color Color = Color.white;
    public int ScoreWhenPotted;
}