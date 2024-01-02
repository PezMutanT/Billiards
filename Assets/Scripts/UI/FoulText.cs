using DG.Tweening;
using UnityEngine;

public class FoulText : MonoBehaviour
{
    [SerializeField] private RectTransform _text;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _animationDuration;
    [SerializeField] private Ease _ease;
    [SerializeField] private float _amplitude;
    [SerializeField] private float _period;
    
    public void AnimateTextIn()
    {
        var originalPosition = _text.transform.localPosition;
        var animationIn = _text.DOAnchorPosX(0f, _animationDuration);
        if (_ease == Ease.Unset)
        {
            animationIn.SetEase(_animationCurve);
        }
        else
        {
            animationIn.SetEase(_ease, _amplitude, _period);
        }
        
        var sequence = DOTween.Sequence();
        sequence.Append(animationIn);
        sequence.AppendInterval(1.5f);
        sequence.Append(_text.DOAnchorPosX(-originalPosition.x, 0.6f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => _text.transform.localPosition = originalPosition));
        sequence.Play();
    }
}
