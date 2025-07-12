using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private GameObject _dailyReward;
    [SerializeField] private GameObject _restart;
    private static GameManager _instance;

    public static GameManager Instance {
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
    }

    private void Start() {
        CheckDailyRewards();
    }

    private void CheckDailyRewards() {
        string lastTime = PlayerPrefs.GetString("LastClaimTime", "");
        if (!string.IsNullOrEmpty(lastTime)) {
            DateTime lastClaimTime = string.IsNullOrEmpty(lastTime) ? DateTime.MinValue : DateTime.Parse(lastTime);
            if (DateTime.Today > lastClaimTime) {
                _dailyReward.SetActive(true);
            }
        }
    }

    public void EnableRestart() { 
        _restart.SetActive(true);
    }
}