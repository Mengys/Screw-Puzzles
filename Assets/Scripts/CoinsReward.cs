using TMPro;
using UnityEngine;

public class CoinsReward : BaseReward
{
    [SerializeField] private GameObject text;
    [SerializeField] private int amount = 100;

    private void Start() {
        text.GetComponent<TextMeshProUGUI>().text = amount.ToString();
    }

    public override void Collect() {
        FindFirstObjectByType<GameSettings>().AddMoney(amount);
    }
}
