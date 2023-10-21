using UnityEngine;

[CreateAssetMenu(menuName = "TableFrictionTestConfiguration")]
public class TableFrictionTestConfiguration : ScriptableObject
{
    public ForceMode ForceMode;
    public Vector3 Force;
    public float ForceDelta;
}
