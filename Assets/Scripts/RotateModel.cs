using DG.Tweening;
using UnityEngine;

public class RotateModel : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float mouseSensitivity = 5f;

    private Tween rotationTween;
    private bool isUserRotating = false;
    private Vector3 lastMousePosition;
    private Vector2 lastTouchPosition;

    private void Start()
    {
        StartRotation();
    }

    private void Update()
    {
        HandleUserInput();
    }

    public void StartRotation()
    {
        rotationTween?.Kill();

        float duration = 360f / rotationSpeed;

        rotationTween = transform.DOLocalRotate(
                new Vector3(0f, 360f, 0f),
                duration,
                RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
    }

    public void StopRotation()
    {
        rotationTween?.Kill();
    }

    private void HandleUserInput()
    {
        // Мышь (ПК)
        if (Input.GetMouseButtonDown(0))
        {
            isUserRotating = true;
            StopRotation();
            lastMousePosition = Input.mousePosition; // сохраняем стартовую позицию
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isUserRotating = false;
            StartRotation();
        }

        if (isUserRotating && Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            float rotX = -delta.x * mouseSensitivity * 0.01f;
            float rotY = delta.y * mouseSensitivity * 0.01f;

            transform.Rotate(Vector3.up, rotX, Space.World);
            transform.Rotate(Vector3.right, rotY, Space.World);
        }

        // Тач (мобильные устройства)
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isUserRotating = true;
                StopRotation();
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved && isUserRotating)
            {
                Vector2 delta = touch.position - lastTouchPosition;
                lastTouchPosition = touch.position;

                float rotX = -delta.x * 0.1f;
                float rotY = delta.y * 0.1f;

                transform.Rotate(Vector3.up, rotX, Space.World);
                transform.Rotate(Vector3.right, rotY, Space.World);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isUserRotating = false;
                StartRotation();
            }
        }
    }

}
