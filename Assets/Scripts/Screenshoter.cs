using UnityEngine;

public class Screenshoter : MonoBehaviour
{
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {

            string fileName = "Screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";

            ScreenCapture.CaptureScreenshot(fileName);
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            Time.timeScale = 0f;
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            Time.timeScale = 1f;
        }
    }
}
