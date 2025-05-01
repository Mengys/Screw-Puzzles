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
    [HideInInspector] public MeshRenderer mesh;

    private bool isActivated = false;
    private Transform myTransform;

    //[Header("Other Settings")]

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

        isActivated = true;
        BoltMoving();
        model.StopRotation();
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
            .OnComplete(() => isEndAnimation = true);
    }
}
