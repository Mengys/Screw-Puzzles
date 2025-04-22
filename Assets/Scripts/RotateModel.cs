using DG.Tweening;
using UnityEngine;

public class RotateModel : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f;

    private void Start()
    {
        transform.DOLocalRotate(new Vector3(0, 0, 360), 360f / rotationSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
    }
}