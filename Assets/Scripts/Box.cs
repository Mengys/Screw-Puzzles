using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public List<Transform> targets = new List<Transform>();
    public int countBox = 0;
    public string color;

    [HideInInspector] public bool isComplete = false;

    private RectTransform rectTransform;
    private Vector3 originalScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    public void AddBoltToBox(Bolt bolt)
    {
        if (countBox >= targets.Count) return;

        Transform targetPoint = targets[countBox];
        countBox++;

        bolt.transform.SetParent(this.transform);
        bolt.transform.DOMove(targetPoint.position, 0.5f)
            .SetEase(Ease.OutBack);

        rectTransform.DOKill();
        rectTransform.localScale = originalScale;

        rectTransform.DOScale(originalScale * 1.1f, 0.1f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                rectTransform.DOScale(originalScale, 0.1f).SetEase(Ease.InQuad);
            });
    }


    public Transform GetTargetFromBox(Box box)
    {
        switch (countBox)
        {
            case 0: return targets[0].transform;
            case 1: return targets[1].transform;
            case 2: return targets[2].transform;
                default: return null;
        }
    }

    public bool HasFreeSpace()
    {
        return countBox < targets.Count;
    }

}
