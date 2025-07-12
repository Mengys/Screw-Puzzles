using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    private static Chest _instance;

    [SerializeField] private Button button;
    [SerializeField] private GameObject text;

    private int boltsUnscrewed = 0;
    private int questBolts;

    public static Chest Instance {
        get {
            return _instance; 
        }
    }

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(gameObject);
        }

        DisableButton();
        questBolts = RandomQuest();
    }

    private void Start() {
        SetText();
    }

    public void AddBolt() {
        boltsUnscrewed++;
        if (boltsUnscrewed >= questBolts) {
            boltsUnscrewed = questBolts;
            EnableButton();
        }
        SetText();
    }

    private void SetText() {
        text.GetComponent<TextMeshProUGUI>().text = $"{boltsUnscrewed}/{questBolts}";
    }

    public void EnableButton() {
        button.enabled = true;
    }

    public void DisableButton() {
        button.enabled = false;
    }

    internal void OnRewardsCollected() {
        DisableButton();
        boltsUnscrewed = 0;
        questBolts = RandomQuest();
        SetText();
    }

    private int RandomQuest() {
        return Random.Range(10, 20);
    }
}
