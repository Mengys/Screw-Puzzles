using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using Solo.MOST_IN_ONE;
using static Solo.MOST_IN_ONE.Most_HapticFeedback;

public class Bolt : MonoBehaviour {
    [Header("Bolt Settings")]
    [SerializeField] private float rotationAmount = 720f;
    [SerializeField] private float rotationDuration = 2f;

    [SerializeField] private float moveDistance = 0.2f;
    [SerializeField] private float moveDuration = 2f;

    [SerializeField] private ColorsEnum color = ColorsEnum.red;

    [SerializeField] private Transform unscrewFailPosition;

    [HideInInspector] public bool isEndAnimation = false;
    [HideInInspector] public bool isScrewing = false;
    [HideInInspector] public MeshRenderer mesh;
    [HideInInspector] public RectTransform targetObject;
    [HideInInspector] public Transform targetHole;

    private bool isUnscrewable = false;
    private bool isMouseDown = false;
    private bool isActivated = false;
    private Transform myTransform;

    private RotateModel model;
    private SoundManager sound;

    private void Start() {
        myTransform = transform;

        mesh = GetComponent<MeshRenderer>();
        mesh.material = SetMaterialColor();

        model = FindObjectOfType<RotateModel>();
        sound = FindObjectOfType<SoundManager>();
    }

    private Material RandomMaterial() {
        Color[] colors = new Color[]
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
            Color.magenta
        };

        Color randomColor = colors[Random.Range(0, colors.Length)];

        var material = new Material(Shader.Find("Standard"));
        material.color = randomColor;
        return material;
    }

    private Material SetMaterialColor() {

        Color randomColor = Color.red;

        switch (color) {
            case ColorsEnum.red:
                randomColor = new Color(0.8566037f, 0.03717327f, 0.03717327f);
                break;
            case ColorsEnum.green:
                randomColor = new Color(0, 0.7960784f, 0.2261874f);
                break;
            case ColorsEnum.blue:
                randomColor = new Color(0, 0.4671441f, 0.7962264f);
                break;
            case ColorsEnum.yellow:
                randomColor = new Color(0.8716981f, 0.7481582f, 0.09045915f);
                break;
            case ColorsEnum.magenta:
                randomColor = new Color(0.8490566f, 0.07529365f, 0.7003093f);
                break;
        }

        var material = new Material(Shader.Find("Standard"));
        material.color = randomColor;
        return material;
    }

    private void OnMouseDown() {
        isMouseDown = true;
        isUnscrewable = true;
    }

    private void OnMouseEnter() {
        if (!isMouseDown) {
            isUnscrewable = false;
        }
    }

    private void OnMouseExit() {
        isUnscrewable = false;
    }

    private void OnMouseUp() {
        isMouseDown = false;
        Unscrew();
        isUnscrewable = true;
    }

    public void Unscrew() {
        if (!isUnscrewable) return;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
        if (EventSystem.current != null && Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;

        if (!IsFree()) {
            CantMoveAnimation();
            return;
        }
        if (isActivated) return;

        if (FindFirstObjectByType<GameSettings>().IsVibrationEnable)
            Most_HapticFeedback.Generate(HapticTypes.LightImpact);

        isScrewing = true;

        isActivated = true;
        BoltMoving();

        Chest.Instance.AddBolt();

        var tutorial = FindFirstObjectByType<TutorialStart>();
        if (tutorial != null) tutorial.UnscrewBolt();
    }

    private void CantMoveAnimation() {
        Sequence seq = DOTween.Sequence();

        float moveDistance = 100;

        switch (FindFirstObjectByType<GameSettings>().GetLevelNumber()) {
            case 1:
                moveDistance = 100;
                break;
            case 2:
                moveDistance = 100;
                break;
            case 3:
                moveDistance = 3;
                break;
            case 4:
                moveDistance = 3;
                break;
            case 5:
                moveDistance = 3;
                break;
            case 6:
                moveDistance = 3;
                break;
            default:
                moveDistance = 3;
                break;
        }

        Vector3 startPosition = transform.localPosition;
        Debug.Log(transform.localPosition);
        seq.Append(transform.DOLocalMove(transform.localPosition + (transform.localRotation * Vector3.forward).normalized * moveDistance, 0.5f)
                .SetEase(Ease.InOutQuad))
            .Append(transform.DOLocalMove(startPosition, 0.5f)
                .SetEase(Ease.InOutQuad));
    }

    private bool IsFree() {
        RaycastHit hit;
        Vector3 origin = transform.position + transform.forward * 4.5f;
        Vector3 direction = transform.forward;
        float maxDistance = 4f;
        if (Physics.Raycast(origin, direction, out hit, maxDistance)) {
            return false;
        }
        return true;
    }

    public void MagnetBolt() {
        if (isActivated) return;

        isScrewing = true;

        isActivated = true;
        BoltMoving();
    }

    private void BoltMoving() {
        sound.PlaySFX();

        //Выкручивание
        transform.DOMove(transform.position + transform.forward * 5, moveDuration)
            .SetEase(Ease.InOutQuad);

        transform.DOLocalRotate(
                transform.localEulerAngles + new Vector3(0f, 0f, rotationAmount),
                rotationDuration,
                RotateMode.FastBeyond360
            )
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => {
                isEndAnimation = true;
            });
    }

    public ColorsEnum GetColorEnum() {
        return color;
    }

    public string GetColorName() {
        switch (color) {
            case ColorsEnum.red:
                return "red";
            case ColorsEnum.green:
                return "green";
            case ColorsEnum.blue:
                return "blue";
            case ColorsEnum.yellow:
                return "yellow";
            case ColorsEnum.magenta:
                return "magenta";
        }
        return "";
    }
}
