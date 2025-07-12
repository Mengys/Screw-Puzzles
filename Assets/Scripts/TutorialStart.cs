using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialStart : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject targetRotationTutorial;
    [SerializeField] private GameObject targetZoomTutorial;
    [SerializeField] private ModelRotation modelRotation;

    private bool RotationTutorialComplited = false;
    private bool ZoomTutorialStarted = false;
    private bool ZoomTutorialComplited = false;
    private GameObject handTarget;
    private int unscrewedBolts = 0;
    private bool unscrewTutorial = true;

    private Sequence seq;
    private void Start() {
        seq = DOTween.Sequence();
        if (PlayerPrefs.GetInt("TutorialStartCompleted") == 1) {
            Destroy(gameObject);
            return;
        }
        if (PlayerPrefs.HasKey("CurrentLevel")) {
            if (PlayerPrefs.GetInt("CurrentLevel") != 1) {
                Destroy(gameObject);
                return;
            }
        }
        FindTarget();
        modelRotation.Disable();
    }

    private void Update() {
        if (unscrewTutorial) { 
            hand.transform.position = handTarget.transform.position + new Vector3(1,-3,-1.5f);
        }
    }

    private void FindTarget() {
        var targets = FindFirstObjectByType<TutorialTargets>().GetComponent<TutorialTargets>();
        handTarget = targets.GetNextTarget();
    }

    public void UnscrewBolt() {
        unscrewedBolts++;
        FindTarget();
        if (unscrewedBolts == 3) {
            StartRotateTutorial();
        }
    }

    public void StartRotateTutorial() {
        hand.GetComponent<Hand>().StopPulse();
        modelRotation.Enable();
        unscrewTutorial = false;
        var pos = targetRotationTutorial.transform.position;
        pos.x = 0;
        hand.transform.position = pos;

        text.GetComponent<TextMeshProUGUI>().text = "Rotate and look through objects by holding your finger on the screen";
        seq = DOTween.Sequence();
        seq.Append(hand.transform.DOMoveX(10f, 2).SetEase(Ease.InOutCubic))
            .Append(hand.transform.DOMoveX(0f, 2).SetEase(Ease.InOutCubic))
            .SetLoops(-1);

        DOVirtual.DelayedCall(1f, () => RotationTutorialComplited = true);
    }

    public void StartZoomTutorial() {
        if (!RotationTutorialComplited) return;
        if (ZoomTutorialStarted) return;
        ZoomTutorialStarted = true;
        seq?.Kill();
        var pos = targetZoomTutorial.transform.position;
        hand.transform.position = pos;

        text.GetComponent<TextMeshProUGUI>().text = "Move the slider to change the zoom";

        seq = DOTween.Sequence();
        seq.Append(hand.transform.DOMoveY(hand.transform.position.y + 5f, 1).SetEase(Ease.InOutCubic))
            .Append(hand.transform.DOMoveY(hand.transform.position.y, 1).SetEase(Ease.InOutCubic))
            .SetLoops(-1);

        DOVirtual.DelayedCall(1f, () => ZoomTutorialComplited = true);
    }

    public void HideTutorial() {
        PlayerPrefs.SetInt("TutorialStartCompleted", 1);
        if (ZoomTutorialComplited)
            Destroy(gameObject);
    }
}
