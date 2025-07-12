using System;
using UnityEngine;

public class DailyRewardConfirm : MonoBehaviour
{
    private GameObject reward;
    public void SetReward(GameObject obj) {
        reward = obj;
    }

    public void CollectReward() {
        PlayerPrefs.SetString("LastClaimTime", DateTime.Now.ToString());

        int lastReward = PlayerPrefs.GetInt("LastClaimedRewardNumber");
        PlayerPrefs.SetInt("LastClaimedRewardNumber", lastReward + 1);

        reward.GetComponent<BaseReward>().Collect();
        FindFirstObjectByType<DailyRewardContainer>().gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void CollectRewardAdd() {
        PlayerPrefs.SetString("LastClaimTime", DateTime.Now.ToString());

        int lastReward = PlayerPrefs.GetInt("LastClaimedRewardNumber");
        PlayerPrefs.SetInt("LastClaimedRewardNumber", lastReward + 1);

        YandexAd.Instance.ShowRewardAdv();

        reward.GetComponent<BaseReward>().Collect();
        reward.GetComponent<BaseReward>().Collect();
        FindFirstObjectByType<DailyRewardContainer>().gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
