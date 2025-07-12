using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBroomstick : MonoBehaviour
{

    [SerializeField] private List<GameObject> _bolts;
    [SerializeField] private Transform _boltBox;

    public void SendBoltsToBox() {
        foreach (var bolt in _bolts) {
            bolt.transform.DOMove(_boltBox.position, 1);
            bolt.transform.DOScale(0,1);
        }
    }

    public void Deactivate() {
        FindAnyObjectByType<BoostsManager>().RemoveBroomstick(1);
        DOVirtual.DelayedCall(0.2f, () => gameObject.SetActive(false));

    }
}
