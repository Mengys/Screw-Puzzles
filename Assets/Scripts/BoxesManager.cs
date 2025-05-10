using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoxesManager : MonoBehaviour
{
    public List<Box> currentBoxes = new List<Box>();

    [SerializeField] private List<Box> allBoxes = new List<Box>();
    [SerializeField] private GameSettings game;
    [SerializeField] private GameObject shavingsPrefab;

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
        Sequence boxSequence = DOTween.Sequence();

        Vector3 oldPosition = currentBox.transform.position;
        Quaternion oldRotation = currentBox.transform.rotation;
        Transform parent = currentBox.transform.parent;

        GameObject shavings = Instantiate(shavingsPrefab, currentBox.transform);
        shavings.transform.localPosition = new Vector3(0f, 0f, 0.05f);

        Destroy(shavings, 0.7f);

        boxSequence.Append(currentBox.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack))
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                // Удалить старые цели
                foreach (var t in currentBox.targets)
                    Destroy(t.gameObject);

                game.AddMoney(10);

                // Удалить старый бокс
                Destroy(currentBox.gameObject);

                // Получить шаблон и нужный масштаб
                Box newBoxTemplate = GetRandomBox(currentBox);
                Vector3 prefabScale = newBoxTemplate.transform.localScale;

                // Создать новый бокс с масштабом 0
                Box newBox = Instantiate(newBoxTemplate, oldPosition, oldRotation, parent);
                newBox.transform.localScale = Vector3.zero;
                newBox.gameObject.SetActive(true);

                // Обновить список
                currentBoxes.Remove(currentBox);
                currentBoxes.Add(newBox);

                // Анимация масштабирования
                newBox.transform.DOScale(prefabScale, 0.5f).SetEase(Ease.OutBack);
            });
    }

    private Box GetRandomBox(Box excludeBox)
    {
        Box randomBox;
        do
        {
            randomBox = allBoxes[Random.Range(0, allBoxes.Count)];
        } while (randomBox.color == excludeBox.color);

        return randomBox;
    }

    public void ClearBoxes()
    {
        Bolt[] bolts = GetComponentsInChildren<Bolt>();
        foreach (var bolt in bolts)
        {
            if (bolt != null)
                Destroy(bolt.gameObject);
        }
    }
}
