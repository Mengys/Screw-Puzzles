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
            return holes[freeHoles].transform;
        }

        Debug.LogWarning("Нет свободных дырок! Игра окончена.");
        gameOver.gameObject.SetActive(true);
        endLevel.gameObject.SetActive(false);
        return null;
    }

    private void Update()
    {
        CheckAndMoveBoltsToBoxes();
    }

    private void CheckAndMoveBoltsToBoxes()
    {
        if (bolts.Count == 0) return;

        BoxesManager boxManager = FindObjectOfType<BoxesManager>();
        TaskManager taskManager = FindObjectOfType<TaskManager>();

        for (int i = bolts.Count - 1; i >= 0; i--)
        {
            Bolt bolt = bolts[i];
            Box targetBox = boxManager.GetBoxByColor(bolt.ToNameString(bolt.mesh.material.color));

            if (targetBox != null)
            {
                bolts.RemoveAt(i);
                freeHoles++;

                Vector3 targetPos = targetBox.GetTargetFromBox(targetBox).position;

                bolt.transform.DOMove(targetPos + new Vector3(0f, 0f, 5f), 1f)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        // Перемещаем болт в коробку
                        bolt.transform.SetParent(targetBox.transform);
                        bolt.transform.localPosition = Vector3.zero;
                        targetBox.AddBoltToBox(bolt);

                        // Обновляем TaskManager
                        taskManager.ProgressBoltTask(bolt.mesh.material.color);

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
        Vector3 targetPosition = startPosition + new Vector3(0f, 0f, 80f); // смещение назад по оси Z

        DOVirtual.Float(0f, 1f, 0.5f, value =>
        {
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

}
