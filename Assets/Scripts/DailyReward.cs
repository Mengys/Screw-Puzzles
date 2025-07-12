using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyReward : MonoBehaviour {
    [SerializeField] private List<GameObject> rewards;
    [SerializeField] private GameObject timerText;

    private GameObject chosenReward;

    public void SetChosenReward(GameObject obj) {
        chosenReward = obj;
    }

    private void Start() {
        foreach (GameObject obj in rewards) {
            obj.GetComponent<BaseReward>().SetReadyToClaim(false);
        }

        string lastTime = PlayerPrefs.GetString("LastClaimTime", "");
        DateTime lastClaimTime;

        if (!string.IsNullOrEmpty(lastTime)) {
            lastClaimTime = DateTime.Parse(lastTime);
        } else {
            lastClaimTime = DateTime.MinValue;
        }

        if (DateTime.Today > lastClaimTime) {
            int lastReward = PlayerPrefs.GetInt("LastClaimedRewardNumber");
            rewards[lastReward].GetComponent<BaseReward>().SetReadyToClaim(true);
        }
    }

    private void OnEnable() {

        foreach (GameObject obj in rewards) {
            obj.GetComponent<BaseReward>().SetReadyToClaim(false);
        }

        string lastTime = PlayerPrefs.GetString("LastClaimTime", "");
        if (!string.IsNullOrEmpty(lastTime)) {
            timerText.GetComponent<TextMeshProUGUI>().text = GetTimeToNextClaim(DateTime.Parse(lastTime));
        } else {
            timerText.GetComponent<TextMeshProUGUI>().text = "CLAIM!";
            DateTime lastClaimTime;

            if (!string.IsNullOrEmpty(lastTime)) {
                lastClaimTime = DateTime.Parse(lastTime);
            } else {
                lastClaimTime = DateTime.MinValue;
            }

            if (DateTime.Today > lastClaimTime) {
                int lastReward = PlayerPrefs.GetInt("LastClaimedRewardNumber");
                rewards[lastReward].GetComponent<BaseReward>().SetReadyToClaim(true);
            }

        }
    }

    private string GetTimeToNextClaim(DateTime lastTime) {

        //int hours = Mathf.FloorToInt((float)(DateTime.Today.AddDays(1) - DateTime.Now).TotalHours);
        //int minutes = Mathf.FloorToInt((float)(DateTime.Today.AddDays(1) - DateTime.Now).TotalMinutes)%60;
        string format = @"hh\:mm";
        return (DateTime.Today.AddDays(1) - DateTime.Now).ToString(format); 
    }
}
