using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public List<Transform> targets = new List<Transform>();
    public int countBox = 0;
    public int boltCount = 0;
    public string color;

    [HideInInspector] public bool isComplete = false;

    private RectTransform rectTransform;
    private Vector3 originalScale;
    private BoxesManager boxManager;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
        boxManager = transform.parent.parent.gameObject.GetComponent<BoxesManager>();
    }

    public void AddBoltToBox(Bolt bolt)
    {
        if (countBox >= targets.Count) return;

        Transform targetPoint = targets[countBox];
        countBox++;

        bolt.transform.SetParent(this.transform);
        //bolt.transform.position = targetPoint.position;

        StartCoroutine(BoltMove(bolt, targetPoint));

        //bolt.transform.DOMove(targetPoint.position, 0.5f)
        //    .SetEase(Ease.OutBack);
        bolt.transform.DOScale(2000, 0.5f);


        rectTransform.DOKill();
        rectTransform.localScale = originalScale;

        rectTransform.DOScale(originalScale * 1.1f, 0.1f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {
                rectTransform.DOScale(originalScale, 0.1f)
                .SetEase(Ease.InQuad)
                .OnComplete(() => { if (countBox >= 3) boxManager.ChangeBox(gameObject.GetComponent<Box>()); });
            });
    }

    IEnumerator BoltMove(Bolt bolt, Transform target) {
        float distance = 10f;
        while (distance > 0.1f) {
            Vector3 translate = (target.position - bolt.transform.position) / 20;
            distance = (target.position - bolt.transform.position).magnitude;
            var pos = bolt.transform.position;
            pos += translate;
            bolt.transform.position = pos;
            yield return new WaitForEndOfFrame();
        }
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
