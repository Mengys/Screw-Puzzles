using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoxesManager : MonoBehaviour
{
    [SerializeField] private List<Box> boxList = new List<Box>();
    [SerializeField] private List<GameObject> newBoxPrefabs = new List<GameObject>(); // Несколько префабов

    public void ChangeBox(Box oldBox)
    {
        if (!oldBox.isComplete) return;

        boxList.Remove(oldBox);

        oldBox.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            Destroy(oldBox.gameObject);

            GameObject prefabToSpawn = GetRandomUnusedPrefab();
            if (prefabToSpawn == null)
            {
                Debug.LogWarning("Нет доступных новых префабов для замены.");
                return;
            }

            // Создаем новый бокс
            GameObject newBoxObj = Instantiate(prefabToSpawn, oldBox.transform.position, Quaternion.identity, transform);
            newBoxObj.transform.localScale = Vector3.zero;

            // Случайный цвет
            Renderer rend = newBoxObj.GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                rend.material.color = Random.ColorHSV();
            }

            // Анимация появления
            newBoxObj.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);

            // Добавляем в список
            Box newBox = newBoxObj.GetComponent<Box>();
            boxList.Add(newBox);
        });
    }

    private GameObject GetRandomUnusedPrefab()
    {
        // Получаем имена уже используемых боксов
        HashSet<string> usedPrefabNames = new HashSet<string>();
        foreach (var box in boxList)
        {
            usedPrefabNames.Add(box.gameObject.name.Replace("(Clone)", "").Trim());
        }

        // Фильтруем префабы, которые еще не использованы
        List<GameObject> availablePrefabs = new List<GameObject>();
        foreach (var prefab in newBoxPrefabs)
        {
            if (!usedPrefabNames.Contains(prefab.name))
            {
                availablePrefabs.Add(prefab);
            }
        }

        if (availablePrefabs.Count == 0)
            return null;

        // Выбираем случайный
        return availablePrefabs[Random.Range(0, availablePrefabs.Count)];
    }

    public Box GetBoxByColor(Color targetColor)
    {
        foreach (var box in boxList)
        {
            MeshRenderer mesh = box.GetComponentInChildren<MeshRenderer>();
            if (mesh == null) continue;

            Color boxColor = mesh.material.color;

            if (boxColor == targetColor)
            {
                return box;
            }
        }

        Debug.LogWarning("Бокс с нужным цветом не найден");
        return null;
    }
}
