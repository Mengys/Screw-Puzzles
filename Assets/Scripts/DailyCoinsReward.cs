using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyCoinsReward : BaseReward
{
    [SerializeField] private GameObject text;
    [SerializeField] private int amount = 100;
    [SerializeField] private GameObject confirmWindow;
    [SerializeField] private GameObject image;

    [SerializeField] private Button button;

    private void Awake() {
        //button = GetComponent<Button>();
    }

    private void Start() {
        text.GetComponent<TextMeshProUGUI>().text = amount.ToString();
    }

    public override void Collect() {
        FindFirstObjectByType<GameSettings>().AddMoney(amount);
    }

    public override void SetReadyToClaim(bool value) {
        button.enabled = value;
        image.SetActive(!value);
    }

    public void ChoseReward() {
        FindFirstObjectByType<DailyReward>().SetChosenReward(gameObject);
        var obj = Instantiate(confirmWindow, FindFirstObjectByType<GameSettings>().gameObject.transform);
        obj.GetComponent<DailyRewardConfirm>().SetReward(gameObject);
    }
}
