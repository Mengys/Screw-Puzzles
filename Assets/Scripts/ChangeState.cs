using UnityEngine;

public class ChangeState : MonoBehaviour
{
    [SerializeField] private GameObject _grey;

    public void Enable() {
        _grey.SetActive(true);
    }

    public void Disable() {
        _grey.SetActive(false);
    }

    public void ChangeEnable() {

    }
}
