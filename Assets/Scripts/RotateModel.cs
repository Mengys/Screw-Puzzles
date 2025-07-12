using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateModel : MonoBehaviour
{
    [SerializeField] private float idleRotationSpeed = 30f;
    [SerializeField] private float mouseSensitivity = 5f;
    private Tween rotationTween;
    private bool isDraging = false;
    private Vector3 lastMousePosition;
    private Vector2 lastTouchPosition;
    public float damping = 0.95f;
    private Vector2 rotationSpeed = Vector2.zero;
    private Vector2 rotation = Vector2.zero;
    private bool isIdling = true;
    private bool isMouse = false;
    private bool canRotate = true;

    private void Start()
    {
        StartRotation();
    }

    private void Update()
    {
        if (!canRotate) return;
        HandleUserInput();

        if (isMouse) DragModelMouse();
        if (!isMouse) DragModelTouch();
    }

    public void StartRotation()
    {
        rotationTween?.Kill();

        float duration = 360f / idleRotationSpeed * 2;

        rotationTween = transform.DOLocalRotate(new Vector3(0f, 360f, 0f), duration, RotateMode.LocalAxisAdd)
            .SetEase(Ease.InSine)
            .SetDelay(1f)
            .OnComplete(() => {
                transform.DOLocalRotate(
                new Vector3(0f, 360f, 0f),
                duration,
                RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear)
            .SetLoops(-1); });
    }

    public void Disable() {
        canRotate = false;
        rotationTween?.Kill();
    }

    public void Ebable() {
        canRotate = true;
    }

    public void ResetModel() {
        //StopRotation();
        //transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        //StartRotation();
    }

    public void StopRotation()
    {
        isIdling = false;
        rotationTween?.Kill();
    }

    private void HandleUserInput() {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject() && !isDraging) return;
        if (EventSystem.current != null && Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && !isDraging) return;

        HandeMouseInput();
        HandleTouchInput2();
    }

    private void DragModelMouse() {
        if (isDraging) {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            float rotX = -delta.x * mouseSensitivity * 0.05f;
            float rotY = delta.y * mouseSensitivity * 0.05f;
            rotationSpeed = new Vector2(rotX, rotY);

            Vector2 lastRotation = new Vector2(rotX, rotY);

            rotation += rotationSpeed;
        } else {
            Debug.Log("tets");
            rotationSpeed *= damping;
            if (rotationSpeed.magnitude < 0.01f) {
                rotationSpeed = Vector2.zero;
                isDraging = false;
            }
            rotation += rotationSpeed;
        }

        transform.Rotate(Vector3.up, rotationSpeed.x, Space.World);
        transform.Rotate(Vector3.right, rotationSpeed.y, Space.World);

        if (!isIdling && !isDraging && rotationSpeed.magnitude < 0.01f) {
            isIdling = true;
            StartRotation();
        }
    }

    private void DragModelTouch() {
        if (isDraging) {
            Touch touch = Input.GetTouch(0);
            Vector2 delta = touch.position - lastTouchPosition;
            lastTouchPosition = touch.position;

            float rotX = -delta.x * 0.15f;
            float rotY = delta.y * 0.15f;
            rotationSpeed = new Vector2(rotX, rotY);

            Vector2 lastRotation = new Vector2(rotX, rotY);

            rotation += rotationSpeed;
        } else {
            rotationSpeed *= damping;
            if (rotationSpeed.magnitude < 0.01f) {
                rotationSpeed = Vector2.zero;
                isDraging = false;
            }
            rotation += rotationSpeed;
        }

        transform.Rotate(Vector3.up, rotationSpeed.x, Space.World);
        transform.Rotate(Vector3.right, rotationSpeed.y, Space.World);

        if (!isIdling && !isDraging && rotationSpeed.magnitude < 0.01f) {
            isIdling = true;
            StartRotation();
        }
    }

    private void HandleTouchInput2() {
        if (Input.touchCount == 1) {
            isMouse = false;
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) {
                isDraging = true;
                rotationSpeed = Vector2.zero;
                StopRotation();
                lastTouchPosition = touch.position;
            } else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                var tutorial = FindFirstObjectByType<TutorialStart>();
                if (tutorial != null) tutorial.StartZoomTutorial();
                isDraging = false;
            }
        }
        //if (!isMouse) DragModelTouch();
    }

    private void HandeMouseInput() {
        if (Input.GetMouseButtonDown(0)) {
            isMouse = true;
            isDraging = true;
            rotationSpeed = Vector2.zero;
            StopRotation();
            lastMousePosition = Input.mousePosition; // сохраняем стартовую позицию
        } else if (Input.GetMouseButtonUp(0)) {
            var tutorial = FindFirstObjectByType<TutorialStart>();
            if (tutorial != null) tutorial.StartZoomTutorial();
            isDraging = false;
        }
        //if (isMouse) DragModelMouse();
    }
}
