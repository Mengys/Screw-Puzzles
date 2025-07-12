using UnityEngine;

public class ChangeMusicState : MonoBehaviour
{
    [SerializeField] private GameObject _grey;

    private SoundManager _soundManager;

    private void Start() {
        _soundManager = FindAnyObjectByType<SoundManager>();
        _grey.SetActive(!_soundManager.isBackgroundEnabled);
    }

    private void Update() {
        _grey.SetActive(!_soundManager.isBackgroundEnabled);
    }
}
