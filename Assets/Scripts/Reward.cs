using UnityEngine;

public class BaseReward : MonoBehaviour
{
    virtual public void Collect() {
        Debug.Log("�� ��������� ����� collect � �������");
    }

    virtual public void SetReadyToClaim(bool value) {
        Debug.Log("�� ��������� ����� SetReadyToClaim � �������");
    }
}
