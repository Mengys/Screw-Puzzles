using UnityEngine;

public class TutorialBolt : MonoBehaviour {
    [SerializeField] private Transform _hole;

    private void Start() {

        transform.SetParent(_hole);
        transform.localPosition = Vector3.zero + new Vector3(0,0,60);
    }
}
