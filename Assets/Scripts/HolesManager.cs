using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HolesManager : MonoBehaviour
{
    public List<GameObject> holes = new List<GameObject>();
    public List<Bolt> bolts = new List<Bolt>();

    [SerializeField] private GameSettings gameOver;
    [SerializeField] private EndLevel endLevel;
    [SerializeField] private GameObject shavingsPrefab;
    [SerializeField] private GameObject boltBox;
    [SerializeField] private GameObject WarningOneHole;

    private int freeHoles = 5;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public Transform GetfreeHole()
    {
        if (freeHoles > 0 && holes.Count >= freeHoles)
        {
            
            freeHoles--;
            if (freeHoles == 1) WarningOneHole.SetActive(true);

            if (freeHoles == 0) {
                GameManager.Instance.EnableRestart();
                return holes[0].transform;
            }

            return holes[freeHoles].transform;
        }

        Debug.LogWarning("Нет свободных дырок! Игра окончена.");
        gameOver.gameObject.SetActive(true);
        endLevel.gameObject.SetActive(false);
        return null;
    }

    private void Update()
    {
        CheckAndMoveBoltsToBoxesFromBoltBox(boltBox.GetComponent<BoltBox>().bolts);
        CheckAndMoveBoltsToBoxes(bolts);
    }

    private void CheckAndMoveBoltsToBoxesFromBoltBox(List<Bolt> bolts) {
        if (bolts.Count == 0) return;

        BoxesManager boxManager = FindObjectOfType<BoxesManager>();
        TaskManager taskManager = FindObjectOfType<TaskManager>();

        for (int i = bolts.Count - 1; i >= 0; i--) {
            Bolt bolt = bolts[i];
            Box targetBox = boxManager.GetBoxByColor(bolt.GetColorName());

            if (targetBox != null) {
                bolts.RemoveAt(i);

                Vector3 targetPos = targetBox.GetTargetFromBox(targetBox).position;

                bolt.transform.DOScale(1325f, 0.5f);
                bolt.transform.DOMove(targetPos + new Vector3(0f, 0f, 5f), 1f)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(() => {
                        // Перемещаем болт в коробку
                        bolt.transform.SetParent(targetBox.transform);
                        bolt.transform.localPosition = Vector3.zero;
                        targetBox.AddBoltToBox(bolt);

                        // Обновляем TaskManager
                        taskManager.ProgressBoltTask(bolt.GetColorEnum());

                        // Анимация поворота
                        AnimateBoltRotation(bolt);

                        // Эффект стружки
                        SpawnShavings(bolt);

                        // Звук
                        audioSource.Play();
                    });
            }
        }
    }

    private void CheckAndMoveBoltsToBoxes(List<Bolt> bolts)
    {
        if (bolts.Count == 0) return;

        BoxesManager boxManager = FindObjectOfType<BoxesManager>();
        TaskManager taskManager = FindObjectOfType<TaskManager>();

        for (int i = bolts.Count - 1; i >= 0; i--)
        {
            Bolt bolt = bolts[i];
            Box targetBox = boxManager.GetBoxByColor(bolt.GetColorName());

            if (targetBox != null)
            {
                bolts.RemoveAt(i);
                freeHoles++;

                var target = targetBox.GetTargetFromBox(targetBox);
                Vector3 targetPos = target.position;
                var targetRotation = target.rotation.eulerAngles;

                //bolt.transform.DORotate(targetRotation + new Vector3(270f, 0, 0), 0.5f, RotateMode.Fast).SetEase(Ease.InOutSine);
                bolt.transform.DOScale(1325f, 0.5f);
                bolt.transform.DOMove(targetPos + new Vector3(0f, 0f, 5f), 1f)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        // Перемещаем болт в коробку
                        bolt.transform.SetParent(targetBox.transform);
                        //bolt.transform.localPosition = Vector3.zero;
                        targetBox.AddBoltToBox(bolt);

                        // Обновляем TaskManager
                        taskManager.ProgressBoltTask(bolt.GetColorEnum());

                        // Анимация поворота
                        //AnimateBoltRotation(bolt);

                        // Эффект стружки
                        SpawnShavings(bolt);

                        // Звук
                        audioSource.Play();
                    });
            }
        }
    }

    private void AnimateBoltRotation(Bolt bolt)
    {
        Quaternion startRotation = bolt.transform.localRotation;
        Quaternion targetRotation = new Quaternion(-0.541675329f, -0.454519421f, -0.454519421f, 0.541675329f);

        DOVirtual.Float(0f, 1f, 0.5f, value =>
        {
            bolt.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, value);
        }).SetEase(Ease.OutSine);
    }

    private void SpawnShavings(Bolt bolt)
    {
        GameObject shavings = Instantiate(shavingsPrefab, bolt.transform);
        shavings.transform.localPosition = new Vector3(0f, 0f, 0.05f);
        Destroy(shavings, 0.7f);
    }

    public void AnimateHoleBoltRotation(Bolt bolt)
    {
        Quaternion startRotation = bolt.transform.localRotation;
        Quaternion targetRotation = new Quaternion(-1f, 0f, 0f, 0f);

        Vector3 startPosition = bolt.transform.localPosition;
        Vector3 targetPosition = new Vector3(0f, 0f, 60f);
        //Vector3 targetPosition = startPosition + new Vector3(0f, 0f, 80f); // смещение назад по оси Z

        bolt.transform.DOScale(1200, 0.5f);

        DOVirtual.Float(0f, 1f, 0.5f, value => {
            bolt.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, value);
            bolt.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, value);
        }).SetEase(Ease.OutSine);
    }

    public void ClearHoles()
    {
        // Удаляем болты из списка bolts
        foreach (var bolt in bolts)
        {
            if (bolt != null)
                Destroy(bolt.gameObject);
        }

        bolts.Clear();

        // Удаляем дочерние болты из дыр
        foreach (var hole in holes)
        {
            foreach (Transform child in hole.transform)
            {
                Destroy(child.gameObject);
            }
        }

        freeHoles = 5;
    }

    public void SendBoltsToBoltBox() {
        for (int i = 0; i < bolts.Count; i++) {
            boltBox.GetComponent<BoltBox>().AddBolt(bolts[i]);

            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(bolts[i].gameObject.transform.DOMove(bolts[i].gameObject.transform.position + Vector3.back * 3, 0.15f).SetEase(Ease.InOutSine))
                .Join(bolts[i].gameObject.transform.DORotate(new Vector3(0, 180, 0), 0.15f, RotateMode.Fast).SetEase(Ease.InOutSine))
                .Append(bolts[i].gameObject.transform.DOMove(boltBox.transform.position, 1f).SetEase(Ease.InOutSine))
                .Join(bolts[i].gameObject.transform.DOScale(0, 1f).SetEase(Ease.InBack));
        }
        bolts.Clear();
        freeHoles = 5;
    }

    public int FreeHolesCount() {
        return freeHoles;
    }
}
