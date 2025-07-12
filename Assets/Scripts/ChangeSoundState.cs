using UnityEngine;

public class ChangeSoundState : MonoBehaviour
{
    [SerializeField] private GameObject _grey;

    private SoundManager _soundManager;

    private void Start() {
        _soundManager = FindAnyObjectByType<SoundManager>();
        _grey.SetActive(!_soundManager.isSFXEnabled);
    }

    private void Update() {
        _grey.SetActive(!_soundManager.isSFXEnabled);
    }
}
