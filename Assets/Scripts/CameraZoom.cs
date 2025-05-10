using UnityEngine;
using UnityEngine.UI;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Slider slider;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 20f;

    private void Start()
    {
        // »нициализируем слайдер и подписываемс€ на изменение
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.onValueChanged.AddListener(OnSliderChanged);

        // ”станавливаем начальное значение
        UpdateCameraZoom(slider.value);
    }

    private void OnSliderChanged(float value)
    {
        UpdateCameraZoom(value);
    }

    private void UpdateCameraZoom(float normalizedValue)
    {
        float zoom = Mathf.Lerp(minZoom, maxZoom, 1f - normalizedValue); // обратный зум (слайдер вверх Ч ближе)

        if (mainCamera.orthographic)
        {
            mainCamera.orthographicSize = zoom;
        }
        else
        {
            mainCamera.fieldOfView = zoom;
        }
    }
}
