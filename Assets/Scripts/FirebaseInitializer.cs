using DG.Tweening.Core.Easing;
#if !UNITY_WEBGL
using Firebase.Analytics;
using Firebase.Extensions;
#endif
using UnityEngine;

public class FirebaseInitializer : MonoBehaviour {
#if !UNITY_WEBGL
    [HideInInspector] public Firebase.FirebaseApp App;
#endif
    public static FirebaseInitializer Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

#if !UNITY_WEBGL
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                App = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }
#endif

    public void SendEventLevelComplitedTime(int level, float time) {
#if !UNITY_WEBGL
        Debug.Log("SendEventLevelComplitedTime");
        FirebaseAnalytics.LogEvent(
            "LevelComplited",
            new Parameter("Level", level),
            new Parameter("Time", time)
        );
#endif
    }

#if !UNITY_WEBGL
    private void OnApplicationPause(bool pause) {

        FindFirstObjectByType<GameSettings>().GetLevelNumber();
        Debug.Log("SendEventGamePause");
        FirebaseAnalytics.LogEvent(
            "GamePause",
            new Parameter("Level", FindFirstObjectByType<GameSettings>().GetLevelNumber()),
            new Parameter("PlayedTime", Time.time),
            new Parameter("Pause", pause.ToString())
        );
    }
#endif
}
