using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameSettings : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private int levelNumber = 1;

    [Header("Level Settings")]
    [SerializeField] private Image closeChest;
    [SerializeField] private Image openChest;

    [Header("Money Settings")]
    [SerializeField] private TextMeshProUGUI moneyText;
    private int ñurrentMoney;

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
    [SerializeField] GameObject gameParent;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        SaveModelDefaults(models[--levelNumber]);
        levelNumber++;

        if (spawnedModel != null)
            Destroy(spawnedModel);

        GameObject modelPrefab = models[--levelNumber];
        levelNumber++;

        spawnedModel = Instantiate(modelPrefab, modelParent);

        parentBolt = FindObjectOfType<ParentBolt>();

        spawnedModel.transform.localPosition = modelPrefab.transform.localPosition;
        spawnedModel.transform.localRotation = modelPrefab.transform.localRotation;
        spawnedModel.transform.localScale = modelPrefab.transform.localScale;

        mainModel = spawnedModel;

        LevelUpdate();
        AddMoney(10);
    }
    private void SaveModelDefaults(GameObject modelPrefab)
    {
        modelPosition = Vector3.zero;
        modelRotation = modelPrefab.transform.rotation;
        modelScale = modelPrefab.transform.localScale;
    }

    public void ResetGame()
    {
        Initialize();

        closeChest.gameObject.SetActive(true);
        openChest.gameObject.SetActive(false);

        parentBolt.currentCountBolt = 0;
        parentBolt.boltText.text = parentBolt.currentCountBolt.ToString() + " / " + parentBolt.boltAllCount.ToString();
    }

    public void LevelLogic()
    {
        if (parentBolt.boltAllCount == parentBolt.currentCountBolt && parentBolt != null)
        {
            //closeChest.gameObject.SetActive(false);
            //openChest.gameObject.SetActive(true);

            levelNumber++;
            LevelUpdate();

            parentBolt.boltAllCount = parentBolt.GetBoltCount();
            ResetGame();

        }
    }

    private void LevelUpdate()
    {
        levelText.text = "Óðîâåíü: " + levelNumber.ToString();
    }

    public void AddMoney(int value)
    {
        ñurrentMoney += value;
        moneyText.text = ñurrentMoney.ToString();
    }

    private void Update()
    {
        LevelLogic();
    }
}
