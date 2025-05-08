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
        if (countBox < targets.Count)
            return targets[countBox];
        else
            return null;
    }

    public bool HasFreeSpace()
    {
        return countBox < targets.Count;
    }

}
