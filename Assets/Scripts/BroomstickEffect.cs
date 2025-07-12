using DG.Tweening;
using UnityEngine;

public class BroomstickEffect : MonoBehaviour
{
    Sequence seq;

    private void Awake() {
        
    }

    private void OnEnable() {
        seq?.Kill();
        seq = DOTween.Sequence();
        seq.Append(transform.DORotate(new Vector3(0, 0, -30), 0.3f))
            .Append(transform.DORotate(new Vector3(0, 0, 30), 0.3f))
            .Append(transform.DORotate(new Vector3(0, 0, -30), 0.3f))
            .Append(transform.DORotate(new Vector3(0, 0, 30), 0.3f))
            .AppendCallback(() => gameObject.SetActive(false));
    }
}
