using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private int levelNumber = 0;

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
    [SerializeField] ParentBolt parentBolt;

    private void Awake()
    {
        SaveModelDefaults(models[levelNumber]);
        Initialize();
    }

    private void Initialize()
    {
        if (spawnedModel != null)
            Destroy(spawnedModel);

        GameObject modelPrefab = models[levelNumber];

        spawnedModel = Instantiate(modelPrefab, modelParent);

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
        modelScale = Vector3.zero;
        modelPosition = Vector3.zero;
        modelRotation = Quaternion.identity;
    }

    public void LevelLogic()
    {
        if (parentBolt.boltAllCount == parentBolt.currentCountBolt)
        {
            levelNumber++;
            LevelUpdate();
            Initialize();
            parentBolt.currentCountBolt = 0;
            parentBolt.boltText.text = parentBolt.currentCountBolt.ToString() + " / " + parentBolt.boltAllCount.ToString();

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
}
