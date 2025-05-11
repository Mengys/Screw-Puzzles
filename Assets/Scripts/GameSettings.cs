using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
//using YG;

public class GameSettings : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private int levelNumber = 1;

    [Header("Money Settings")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [HideInInspector] public int currentLevelEarnings = 0;
    [HideInInspector] public int currentMoney;
    [Header("Money Animation")]
    [SerializeField] private RectTransform canvasRoot; // Canvas для UI
    [SerializeField] private TextMeshProUGUI flyingTextPrefab; // Префаб "летающего текста"

    [Header("Model Settings")]
    [SerializeField] private GameObject mainModel;
    [SerializeField] private Transform modelParent;
    [SerializeField] private List<GameObject> models;

    private Vector3 modelPosition;
    private Quaternion modelRotation;
    private Vector3 modelScale;
    private GameObject spawnedModel;

    [Header("Other Settings")]
    private ParentBolt parentBolt;
    [SerializeField] private GameObject gameParent;
    [SerializeField] private TaskManager taskManager;
    [SerializeField] private EndLevel endLevel;
    [SerializeField] private GameObject endGame;
    //[SerializeField] private YandexGame sdk;

    private bool levelCompleted = false;
    private bool resetting = false;

    private void Awake()
    {
        Initialize();
        LevelUpdate();
        AddMoney(10);
    }

    private void Initialize()
    {
        if (models == null || models.Count == 0)
        {
            Debug.LogError("Models list is empty or not assigned!");
            return;
        }

        int index = levelNumber - 1;

        if (index < 0 || index >= models.Count)
        {
            Debug.LogError("Invalid level number!");
            return;
        }

        SaveModelDefaults(models[index]);

        if (spawnedModel != null)
            Destroy(spawnedModel);

        GameObject modelPrefab = models[index];
        spawnedModel = Instantiate(modelPrefab, modelParent);

        parentBolt = FindObjectOfType<ParentBolt>();
        if (parentBolt == null)
        {
            Debug.LogError("ParentBolt is not found in the scene!");
            return;
        }

        spawnedModel.transform.localPosition = modelPrefab.transform.localPosition;
        spawnedModel.transform.localRotation = modelPrefab.transform.localRotation;
        spawnedModel.transform.localScale = modelPrefab.transform.localScale;

        mainModel = spawnedModel;
    }

    private void SaveModelDefaults(GameObject modelPrefab)
    {
        modelPosition = Vector3.zero;
        modelRotation = modelPrefab.transform.rotation;
        modelScale = modelPrefab.transform.localScale;
    }

    public void ResetGame()
    {
        // Показ рекламы перед перезапуском уровня
        //sdk._FullscreenShow();

        endGame.SetActive(false);

        FindObjectOfType<HolesManager>().ClearHoles();
        FindObjectOfType<BoxesManager>().ClearBoxes();

        resetting = true;
        levelCompleted = false;

        if (spawnedModel != null)
            Destroy(spawnedModel);

        Initialize();

        if (parentBolt != null)
        {
            parentBolt.currentCountBolt = 0;
            if (parentBolt.boltText != null)
                parentBolt.boltText.text = parentBolt.currentCountBolt + " / " + parentBolt.boltAllCount;
        }

        StartCoroutine(ClearResettingFlag());
    }

    private IEnumerator ClearResettingFlag()
    {
        yield return null; // Ждём 1 кадр
        resetting = false;
    }

    public void LevelLogic()
    {
        if (resetting || levelCompleted)
        {
            return;
        }

        if (parentBolt != null && parentBolt.boltAllCount == parentBolt.currentCountBolt)
        {
            endLevel.StartRoulette();

            levelCompleted = true;
            levelNumber++;
            LevelUpdate();

            if (parentBolt != null)
            {
                parentBolt.boltAllCount = parentBolt.GetBoltCount();
                //ResetGame();
            }
        }
    }

    private void LevelUpdate()
    {
        foreach (var task in taskManager.tasks)
        {
            if (task.type == TaskType.DisassembleBuilding && task.completed)
            {
                Debug.Log("Задание 'Разобрать строение' выполнено!");
                return;
            }
        }

        if (levelText != null)
            levelText.text = "Уровень: " + levelNumber.ToString();
    }


    public void AddMoney(int value)
    {
        currentLevelEarnings += value;

        AnimateMoneyGain(value);
    }


    private void AnimateMoneyGain(int value)
    {
        int startMoney = currentMoney;
        int targetMoney = currentMoney + value;
        currentMoney = targetMoney;

        // 1. Плавная анимация счета
        DOTween.To(() => startMoney, x =>
        {
            moneyText.text = x.ToString();
        }, targetMoney, 1.5f).SetEase(Ease.OutCubic); // увеличена длительность

        // 2. Летающий текст слева
        var flyingText = Instantiate(flyingTextPrefab, canvasRoot);
        flyingText.text = $"+{value}";

        flyingText.transform.position = moneyText.transform.position + new Vector3(-150f, 0, 0);

        flyingText.DOFade(0f, 1.5f).SetEase(Ease.OutQuad); // увеличена длительность
        flyingText.transform.DOMoveX(moneyText.transform.position.x, 1.5f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => Destroy(flyingText.gameObject));
    }




    private void Update()
    {
        LevelLogic();

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    //fullReset

    public void TEST_NextLevel()
    {
        levelCompleted = true;
        levelNumber++;
        LevelUpdate();
        parentBolt.boltAllCount = parentBolt.GetBoltCount();
        ResetGame();
    }
}
