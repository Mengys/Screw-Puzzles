using UnityEngine;

public class BaseReward : MonoBehaviour
{
    virtual public void Collect() {
        Debug.Log("Не переписан метод collect у награды");
    }

    virtual public void SetReadyToClaim(bool value) {
        Debug.Log("Не переписан метод SetReadyToClaim у награды");
    }
}
