using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoxesManager : MonoBehaviour {
    public List<Box> currentBoxes = new List<Box>();

    [SerializeField] private List<Box> allBoxes = new List<Box>();
    [SerializeField] private GameSettings game;
    [SerializeField] private GameObject shavingsPrefab;
    [SerializeField] private GameObject gameParrent;
    [SerializeField] private List<Transform> boxPositions;

    private int boxesCount = 0;

    private void Start() {
        //StartLevel();
    }

    private void Update() {
        
    }

    private GameObject GetNewBox() {
        var boxList = gameParrent.transform.GetChild(0).GetComponent<BoxList>();
        
        return boxList.GetNext();
    }

    public Box GetBoxByColor(string targetColor) {
        foreach (var box in currentBoxes) {
            if (box.color == targetColor && box.boltCount < 3) {
                box.boltCount++;
                return box;
            }
        }

        Debug.LogWarning("Бокс с нужным цветом не найден");
        return null;
    }

    public void ChangeBox(Box currentBox) {
        Sequence boxSequence = DOTween.Sequence();

        Vector3 oldPosition = currentBox.transform.position;
        Quaternion oldRotation = currentBox.transform.rotation;
        Transform parent = currentBox.transform.parent;

        //GameObject shavings = Instantiate(shavingsPrefab, currentBox.transform);
        //shavings.transform.localPosition = new Vector3(0f, 0f, 0.05f);

        //Destroy(shavings, 0.7f);

        boxSequence.Append(currentBox.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack))
            .AppendInterval(1f)
            .AppendCallback(() => {
                // Удалить старые цели
                foreach (var t in currentBox.targets)
                    Destroy(t.gameObject);

                game.AddMoney(10);

                // Удалить старый бокс
                Destroy(currentBox.gameObject);
                currentBoxes.Remove(currentBox);

                CreateNewBox(parent);
            });
    }

    public void CreateNewBox(Transform parent) {
        var boxObject = GetNewBox();
        if (boxObject == null) return;
        Box newBoxTemplate = boxObject.GetComponent<Box>();
        Vector3 prefabScale = newBoxTemplate.transform.localScale;
        Box newBox = Instantiate(newBoxTemplate, Vector3.zero, Quaternion.identity, parent);
        newBox.transform.localPosition = Vector3.zero;
        newBox.transform.localScale = Vector3.zero;
        newBox.transform.localRotation = Quaternion.identity;

        newBox.gameObject.SetActive(true);
        currentBoxes.Add(newBox);
        newBox.transform.DOScale(prefabScale, 0.5f).SetEase(Ease.OutBack);
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

    public void ClearBoxes() {
        foreach (var box in currentBoxes) {
            Destroy(box.gameObject);
        }
        currentBoxes.Clear();
        //Bolt[] bolts = GetComponentsInChildren<Bolt>();
        //foreach (var bolt in bolts) {
        //    if (bolt != null)
        //        Destroy(bolt.gameObject);
        //}
    }

    internal void GetNewBox(GameObject gameObject) {
        Destroy(gameObject);
    }

    public void StartLevel() {
        CreateNewBox(boxPositions[0]);
        CreateNewBox(boxPositions[1]);
        CreateNewBox(boxPositions[2]);
    }

    public Box GetRandomBox() {
        var i = Random.Range(0, currentBoxes.Count);
        return currentBoxes[i];
    }
}
