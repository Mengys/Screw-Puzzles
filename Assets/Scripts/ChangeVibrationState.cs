using UnityEngine;

public class ChangeVibrationState : MonoBehaviour
{
    [SerializeField] private GameObject _grey;

    private GameSettings _gameSettings;

    private void Start() {
        _gameSettings = FindAnyObjectByType<GameSettings>();
        _grey.SetActive(!_gameSettings.IsVibrationEnable);
    }

    private void Update() {
        _grey.SetActive(!_gameSettings.IsVibrationEnable);
    }
}
