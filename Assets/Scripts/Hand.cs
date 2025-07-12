using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Hand : MonoBehaviour
{
    Tween tween;

    private void Start() {
        StartPulse();
    }

    public void StopPulse() {
        tween?.Kill();
    }

    public void StartPulse() {
        tween = transform.DOScale(1.5f, 0.6f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
}
