using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Slider slider;
    [SerializeField] private float minZoom = 40f;
    [SerializeField] private float maxZoom = 60f;
    [SerializeField] private float pinchSensitivity = 0.1f;

    private float currentZoom;

    private void Start()
    {
        // ��������� ��������
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.onValueChanged.AddListener(OnSliderChanged);

        // ��������� ��������
        currentZoom = Mathf.Lerp(minZoom, maxZoom, 1f - slider.value);
        ApplyZoom(currentZoom);
    }

    private void Update()
    {
        HandlePinchZoom();
    }

    private void OnSliderChanged(float value)
    {
        currentZoom = Mathf.Lerp(minZoom, maxZoom, 1f - value);
        ApplyZoom(currentZoom);
        var tutorial = FindFirstObjectByType<TutorialStart>();
        if (tutorial != null) tutorial.HideTutorial();
    }

    private void ApplyZoom(float zoom)
    {
        if (mainCamera.orthographic) {
            mainCamera.orthographicSize = zoom;
        } else {
            mainCamera.fieldOfView = zoom;
        }

        // ���������� �������� (���� ��� ������ �� ���������)
        float normalized = Mathf.InverseLerp(minZoom, maxZoom, zoom);
        slider.SetValueWithoutNotify(1f - normalized);
    }

    private void HandlePinchZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // ������� � ���������� �����
            Vector2 prevTouch0 = touch0.position - touch0.deltaPosition;
            Vector2 prevTouch1 = touch1.position - touch1.deltaPosition;

            float prevMagnitude = (prevTouch0 - prevTouch1).magnitude;
            float currentMagnitude = (touch0.position - touch1.position).magnitude;

            float difference = prevMagnitude - currentMagnitude;

            // ��������� ���
            currentZoom += difference * pinchSensitivity;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            ApplyZoom(currentZoom);
            FindFirstObjectByType<TutorialStart>().HideTutorial();
        }
    }
}
