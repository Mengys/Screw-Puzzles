using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Bolt : MonoBehaviour
{
    [Header("Bolt Settings")]
    [SerializeField] private float rotationAmount = 720f;
    [SerializeField] private float rotationDuration = 2f;

    [SerializeField] private float moveDistance = 0.2f;
    [SerializeField] private float moveDuration = 2f;

    [HideInInspector] public bool isEndAnimation = false;
    private bool isActivated = false;
    private Transform myTransform;

    //[Header("Other Settings")]

    private RotateModel model;
    private SoundManager sound;

    private void Start()
    {
        myTransform = transform;

        MeshRenderer mesh = GetComponent<MeshRenderer>();
        mesh.material = RandomMaterial();

        model = FindObjectOfType<RotateModel>();
        sound = FindObjectOfType<SoundManager>();
    }

    private Material RandomMaterial()
    {
        var color = new Color(Random.value, Random.value, Random.value);
        var material = new Material(Shader.Find("Standard"));
        material.color = color;
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
