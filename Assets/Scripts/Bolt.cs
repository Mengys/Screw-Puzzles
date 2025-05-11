using UnityEngine;
using DG.Tweening;

public class Bolt : MonoBehaviour
{
    [Header("Bolt Settings")]
    [SerializeField] private float rotationAmount = 720f;
    [SerializeField] private float rotationDuration = 2f;

    [SerializeField] private float moveDistance = 0.2f;
    [SerializeField] private float moveDuration = 2f;

    [HideInInspector] public bool isEndAnimation = false;
    [HideInInspector] public bool isScrewing = false;
    [HideInInspector] public MeshRenderer mesh;
    [HideInInspector] public RectTransform targetObject;
    [HideInInspector] public Transform targetHole;

    private bool isActivated = false;
    private Transform myTransform;

    private RotateModel model;
    private SoundManager sound;

    private void Start()
    {
        myTransform = transform;

        mesh = GetComponent<MeshRenderer>();
        mesh.material = RandomMaterial();

        model = FindObjectOfType<RotateModel>();
        sound = FindObjectOfType<SoundManager>();
    }

    private Material RandomMaterial()
    {
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

    private void OnMouseDown()
    {
        if (isActivated) return;

        isScrewing = true;

        isActivated = true;
        BoltMoving();
        //model.StopRotation();
    }

    private void BoltMoving()
    {
        sound.PlaySFX();

        myTransform.DOLocalMove(myTransform.localPosition + Vector3.up * moveDistance, moveDuration)
            .SetEase(Ease.InOutQuad);

        myTransform.DOLocalRotate(
                myTransform.localEulerAngles + new Vector3(0f, 0f, rotationAmount),
                rotationDuration,
                RotateMode.FastBeyond360
            )
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                isEndAnimation = true;
            });
    }

    public string ToNameString(Color color)
    {
        if (color == Color.red) return "red";
        if (color == Color.blue) return "blue";
        if (color == Color.green) return "green";
        if (color == Color.yellow) return "yellow";
        if (color == Color.white) return "white";
        if (color == Color.black) return "black";
        if (color == Color.cyan) return "cyan";
        if (color == Color.magenta) return "magenta";
        if (color == Color.gray) return "gray";

        return "#" + ColorUtility.ToHtmlStringRGB(color);
    }
}
