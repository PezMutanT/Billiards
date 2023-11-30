using DG.Tweening;
using UnityEngine;

public class CurrentPlayerMarker : MonoBehaviour
{
    public void Activate()
    {
        gameObject.SetActive(true);
        transform.DOScale(1.2f, 0.4f).SetLoops(-1, LoopType.Yoyo);
    }

    public void Deactivate()
    {
        transform.DORewind();
        gameObject.SetActive(false);
    }
}
