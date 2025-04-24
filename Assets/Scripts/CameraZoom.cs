using EasyMobileInput;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Joystick rightJoystick;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 20f;

    private void Update()
    {
        float verticalInput = rightJoystick.CurrentProcessedValue.y;

        float zoomDelta = verticalInput * zoomSpeed * Time.deltaTime;

        if (mainCamera.orthographic)
        {
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - zoomDelta, minZoom, maxZoom);
        }
        else
        {
            mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView - zoomDelta, minZoom, maxZoom);
        }
    }
}
