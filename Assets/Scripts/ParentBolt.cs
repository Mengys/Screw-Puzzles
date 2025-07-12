using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

public class ParentBolt : MonoBehaviour
{
    [SerializeField] public List<Bolt> boltList = new List<Bolt>();
    [SerializeField] public TextMeshProUGUI boltText;
    [SerializeField] public GameObject shavingsPrefab;

    [HideInInspector] public int currentCountBolt = 0;
    [HideInInspector] public int boltAllCount = 0;

    private Transform targetObject;
    private Vector3 targetWorldPos;
    private AudioSource audioSource;
    private HolesManager holesManager;

    List<Action> actions = new List<Action>();

    private void Start()
    {
        InitializeManagers();
        PopulateBoltList();
    }

    private void Update()
    {
        CheckBoltAnimations();

        foreach (var action in actions) {
            action();
        }
    }

    private void InitializeManagers()
    {
        holesManager = FindObjectOfType<HolesManager>();
        audioSource = GetComponent<AudioSource>();
        GameObject textObject = GameObject.FindGameObjectWithTag("Bolt Count");
        boltText = textObject.GetComponent<TextMeshProUGUI>();
    }

    private void PopulateBoltList() {
        foreach (Transform child in transform) {

            Bolt bolt = child.GetComponent<Bolt>();
            if (bolt != null) {
                boltList.Add(bolt);
            }
        }

        boltAllCount = boltList.Count;
        UpdateBoltText();
    }

    public void UseMagnet() {
        var boxesManager = FindObjectOfType<BoxesManager>();
        var box = boxesManager.GetRandomBox();
        var color = box.color;
        var freeHoles = 3 - box.boltCount;

        foreach (var bolt in boltList) {
            if (freeHoles > 0 && color == bolt.GetColorName()) {
                freeHoles--;
                bolt.MagnetBolt();
            }
        }
    }

    private void CheckBoltAnimations()
    {
        for (int i = boltList.Count - 1; i >= 0; i--)
        {
            Bolt bolt = boltList[i];
            if (!bolt.isEndAnimation)
                continue;

            bolt.isEndAnimation = false;

            try
            {
                ProcessBoltAnimation(bolt);
            }
            catch (NullReferenceException ex)
            {
                Debug.LogWarning($"Ошибка при обработке анимации болта: {ex.Message}. Повторная попытка...");

                try
                {
                        ProcessBoltAnimation(bolt);
                }
                catch (NullReferenceException ex2)
                {
                    Debug.LogError($"Не удалось обработать болт повторно: {ex2.Message}");
                    while (true)
                        ProcessBoltAnimation(bolt);
                }
            }
        }
    }


    private void ProcessBoltAnimation(Bolt bolt)
    {
        BoxesManager boxManager = FindFirstObjectByType<BoxesManager>();
        Box box = boxManager.GetBoxByColor(bolt.GetColorName());

        var targetObject = GetTargetTransform(box, out Transform holeUsed);

        // Установка позиции назначения
        targetWorldPos = targetObject.position;
        var targetRotation = targetObject.rotation.eulerAngles;

        // Сохраняем дырку, если используется
        bolt.targetHole = holeUsed;

        bolt.GetComponent<Collider>().enabled = false;

        if (holeUsed != null) {
            var rotateTween = bolt.transform.DORotate(targetRotation + new Vector3(180f, 0, 0), 0.5f, RotateMode.Fast).SetEase(Ease.InOutSine);
        } else {
            var rotateTween = bolt.transform.DORotate(targetRotation, 0.5f, RotateMode.Fast).SetEase(Ease.InOutSine);
        }

        Vector3 move = (targetWorldPos - bolt.transform.position);
        bolt.transform.Translate(move);

        var moveTween = bolt.transform.DOMove(targetWorldPos + new Vector3(0f, 0f, -10f), 0.5f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => OnBoltAnimationComplete(bolt, box));
    }

    private Transform GetTargetTransform(Box box, out Transform usedHole)
    {
        if (box != null)
        {
            usedHole = null;
            return box.GetTargetFromBox(box) as RectTransform;
        }

        usedHole = holesManager.GetfreeHole() as Transform;
        return usedHole;
    }

    private void OnBoltAnimationComplete(Bolt bolt, Box box)
    {
        boltList.Remove(bolt);
        currentCountBolt++;
        UpdateBoltText();

        FindObjectOfType<TaskManager>().ProgressBoltTask(bolt.GetColorEnum());
        //AnimateBoltRotation(bolt);
        SpawnShavings(bolt);

        if (box != null)
        {
            bolt.transform.SetParent(box.transform);
            box.AddBoltToBox(bolt);
        }
        else if (bolt.targetHole != null)
        {
            bolt.transform.SetParent(bolt.targetHole);
            //bolt.transform.localPosition = new Vector3(0f, 0f, -20f); // Смещение к игроку

            if (holesManager != null)
            {
                holesManager.bolts.Add(bolt);
                holesManager.AnimateHoleBoltRotation(bolt);
            }
        }

        audioSource.Play();
    }

    private void AnimateBoltRotation(Bolt bolt)
    {
        Quaternion startRotation = bolt.transform.localRotation;
        Quaternion targetRotation = new Quaternion(-0.541675329f, -0.454519421f, -0.454519421f, 0.541675329f);

        DOVirtual.Float(0f, 1f, 0.5f, value => {
            bolt.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, value);
        }).SetEase(Ease.OutSine);
    }

    private void SpawnShavings(Bolt bolt)
    {
        GameObject shavings = Instantiate(shavingsPrefab, bolt.transform);
        shavings.transform.localPosition = new Vector3(0f, 0f, 0.05f);
        Destroy(shavings, 0.7f);
    }

    private void UpdateBoltText()
    {
        boltText.text = $"{currentCountBolt} / {boltAllCount}";
    }

    public int GetBoltCount()
    {
        return boltList.Count;
    }
}
