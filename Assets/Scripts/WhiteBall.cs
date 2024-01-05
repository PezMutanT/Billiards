using UnityEngine;

public class WhiteBall : Ball
{
    protected override Vector3 GetInitialPosition()
    {
        return new Vector3(11.6f, 0f, 1.2f);
    }
}
