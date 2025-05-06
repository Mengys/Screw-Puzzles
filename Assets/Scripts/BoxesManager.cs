using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoxesManager : MonoBehaviour
{
    public List<Box> currentBoxes = new List<Box>();

    [SerializeField] private List<Box> allBoxes = new List<Box>();

    public Box GetBoxByColor(string targetColor)
    {
        foreach (var box in currentBoxes)
        {
            if (box.color == targetColor) return box;
        }

        Debug.LogWarning("Бокс с нужным цветом не найден");
        return null;
    }

    public void ChangeBox(Box currentBox)
    {
        currentBox.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            currentBox.gameObject.SetActive(false);

            Box newBox = GetRandomBox(currentBox);

            newBox.transform.localScale = Vector3.zero;
            newBox.gameObject.SetActive(true);

            newBox.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        });
    }

    private Box GetRandomBox(Box excludeBox)
    {
        Box randomBox;
        do
        {
            randomBox = allBoxes[Random.Range(0, allBoxes.Count)];
        } while (randomBox == excludeBox);

        return randomBox;
    }
}
