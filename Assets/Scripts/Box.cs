using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public List<Transform> targets = new List<Transform>();
    public int countBox = 0;
    public string color;

    [HideInInspector] public bool isComplete = false;

    public void AddBoltToBox(Bolt bolt)
    {
        if (countBox >= targets.Count) return;

        Transform targetPoint = targets[countBox];
        countBox++;

        // Переместить болт к нужной точке (локально относительно коробки)
        bolt.transform.SetParent(this.transform); // Привязать болт к коробке
        bolt.transform.DOMove(targetPoint.position, 0.5f)
            .SetEase(Ease.OutBack);

        if (countBox == targets.Count)
        {
            isComplete = true;
            GetComponentInParent<BoxesManager>().ChangeBox(this);
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
}
