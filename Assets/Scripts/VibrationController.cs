using UnityEngine;

public static class VibrationController
{
    static AndroidJavaClass unityPlayer;
    static AndroidJavaObject currentActivity;
    static AndroidJavaObject vibrator;
    static AndroidJavaClass vibrationEffect;
    static bool initialized = false;

    public static void Init() {
        if (initialized) return;
        if (Application.isMobilePlatform) {
            unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
            if (AndroidVersion >= 26)
                vibrationEffect = new AndroidJavaClass("android.os.VibrationEffect");
            initialized = true;
        }
    }

    public static int AndroidVersion {
        get {
            if (Application.platform != RuntimePlatform.Android) return 0;
            string os = SystemInfo.operatingSystem;
            int sdkPos = os.IndexOf("API-");
            return int.Parse(os.Substring(sdkPos + 4, 2));
        }
    }

    public static void Vibrate(long milliseconds, int amplitude = -1) {
        Init();
        if (!Application.isMobilePlatform) return;

        if (AndroidVersion >= 26) {
            AndroidJavaObject effect = vibrationEffect.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, amplitude);
            vibrator.Call("vibrate", effect);
        } else {
            vibrator.Call("vibrate", milliseconds);
        }
    }
}