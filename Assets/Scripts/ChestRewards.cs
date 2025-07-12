using System.Collections.Generic;
using UnityEngine;

public class ChestRewards : MonoBehaviour
{
    [SerializeField] private List<GameObject> rewardTypes;
    [SerializeField] private GameObject container;
    private List<GameObject> rewards = new List<GameObject>();

    public void SpawnRewards() {
        int rewardsNumber = Random.Range(1, 5);

        for (int i = 0; i < rewardsNumber; i++) {
            var obj = Instantiate(rewardTypes[Random.Range(0, rewardTypes.Count)], gameObject.transform);
            rewards.Add(obj);
        }
    }

    public void CollectRewards() {
        foreach (var reward in rewards) {
            reward.GetComponent<BaseReward>().Collect();
            Destroy(reward);
        }
        rewards.Clear();
        container.SetActive(false);

        Chest.Instance.OnRewardsCollected();
    }
}
