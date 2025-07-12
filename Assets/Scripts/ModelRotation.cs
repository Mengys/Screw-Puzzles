using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModelRotation : MonoBehaviour
{
    private float _maxSpeed = 0.1f;
    public Vector2 _currentSpeed = Vector2.zero;
    private float _acceleration = 0.05f;
    private bool _isIdle = true;
    private bool _isDraging = false;
    private Vector2 _pointerPosition = Vector2.zero;
    private Vector2 _prevPointerPosition = Vector2.zero;
    private float _sensitivity = 0.1f;
    private Vector2 _rotation;
    private float _damping = 0.95f;
    private float _timer = 1.5f;
    private bool _isEnabled = true;
    public void ResetModel() {
        _currentSpeed = Vector2.zero;
        _isDraging = false;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void Enable() {
        _isEnabled = true;
    }

    public void Disable() {
        _isEnabled = false;
    }

    private void Start() {
        StartIdleRotation();
        _isIdle = true;
    }

    private void Update() {
        if (!_isEnabled) return;
        HandleUserInput();
        Rotate();
    }

    private void StartIdleRotation() {
        _isIdle = true;
        _timer = 1f;
        _currentSpeed = Vector2.zero;
    }

    private void StopIdleRotation() {
        _isIdle = false;
    }

    private void Rotate() {
        if (_isDraging) {

            Vector3 delta = _pointerPosition - _prevPointerPosition;
            _prevPointerPosition = _pointerPosition;

            float rotX = -delta.x * _sensitivity;
            float rotY = delta.y * _sensitivity;
            _currentSpeed = new Vector2(rotX, rotY);

            Vector2 lastRotation = new Vector2(rotX, rotY);

            _rotation += _currentSpeed;
        } else if (!_isIdle) {
            _currentSpeed *= _damping;
            if (_currentSpeed.magnitude < 0.01f) {
                _currentSpeed = Vector2.zero;
                _isDraging = false;
            }
            _rotation += _currentSpeed;
        }

        if (!_isIdle && !_isDraging && _currentSpeed.magnitude < 0.01f) {
            StartIdleRotation();
        }

        if (_isIdle) {
            _timer -= Time.deltaTime;
            if (_timer < 0f) {
                _currentSpeed.x += _acceleration * Time.deltaTime;
                if (_currentSpeed.x > _maxSpeed)
                    _currentSpeed.x = _maxSpeed;
            }
        }

        transform.Rotate(Vector3.up, _currentSpeed.x, Space.World);
        transform.Rotate(Vector3.right, _currentSpeed.y, Space.World);
    }

    private void HandleUserInput() {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject() && !_isDraging) return;
        if (EventSystem.current != null && Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && !_isDraging) return;

        HandeMouseInput();
        HandleTouchInput();
    }

    private void HandeMouseInput() {
        if (Input.GetMouseButtonDown(0)) {
            _isDraging = true;
            _currentSpeed = Vector2.zero;
            StopIdleRotation();
            _pointerPosition = Input.mousePosition;
            _prevPointerPosition = Input.mousePosition;
        } else if (Input.GetMouseButtonUp(0)) {
            var tutorial = FindFirstObjectByType<TutorialStart>();
            if (tutorial != null) tutorial.StartZoomTutorial();
            _isDraging = false;
        } else if (Input.GetMouseButton(0)) {
            _pointerPosition = Input.mousePosition;
        }
    }

    private void HandleTouchInput() {
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) {
                _isDraging = true;
                _currentSpeed = Vector2.zero;
                StopIdleRotation();
                _pointerPosition = touch.position;
                _prevPointerPosition = touch.position;
            } else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                var tutorial = FindFirstObjectByType<TutorialStart>();
                if (tutorial != null) tutorial.StartZoomTutorial();
                _isDraging = false;
            } else if (touch.phase == TouchPhase.Moved) {
                _pointerPosition = touch.position;
            }
        }
    }
}
