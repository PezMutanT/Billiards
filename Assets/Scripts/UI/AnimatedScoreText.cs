using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimatedScoreText : MonoBehaviour
{
    private TextMeshProUGUI _animatedScoreText;
    private Vector3 _initialPosition;
    
    public void Init()
    {
        gameObject.SetActive(false);
        _animatedScoreText = GetComponent<TextMeshProUGUI>();
        _initialPosition = _animatedScoreText.transform.localPosition;
    }

    public void AnimateScoreText(int deltaScore)
    {
        var prefix = deltaScore > 0 ? "+" : "";
        _animatedScoreText.gameObject.SetActive(true);
        _animatedScoreText.text = $"{prefix}{deltaScore}";
        _animatedScoreText.transform.DOLocalMoveY(150f, 1.5f)
            .OnComplete(() =>
            {
                OnTextAnimationCompleted(_animatedScoreText);
            });
    }

    private void OnTextAnimationCompleted(TextMeshProUGUI animatedText)
    {
        animatedText.gameObject.SetActive(false);
        _animatedScoreText.transform.localPosition = _initialPosition;
    }
}
