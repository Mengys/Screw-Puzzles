using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private Image backgroundIcon;
    [SerializeField] private Image sfxIcon;

    public bool isSFXEnabled = true;
    public bool isBackgroundEnabled = true;

    private void Awake()
    {
        //sfxIcon.gameObject.SetActive(!isSFXEnabled);
        //backgroundIcon.gameObject.SetActive(!isBackgroundEnabled);

        sfxSource.gameObject.SetActive(isSFXEnabled);
        backgroundMusic.gameObject.SetActive(isBackgroundEnabled);

        if (isBackgroundEnabled)
            backgroundMusic.Play();
    }


    public void PlaySFX()
    {
        if (isSFXEnabled)
            sfxSource.Play();
    }

    public void ToggleSFX()
    {
        isSFXEnabled = !isSFXEnabled;

        //sfxIcon.gameObject.SetActive(!isSFXEnabled);
        sfxSource.gameObject.SetActive(isSFXEnabled);
    }

    public void ToggleBackgroundMusic()
    {
        isBackgroundEnabled = !isBackgroundEnabled;

        //backgroundIcon.gameObject.SetActive(!isBackgroundEnabled);
        backgroundMusic.gameObject.SetActive(isBackgroundEnabled);

        if (isBackgroundEnabled)
            backgroundMusic.Play();
        else
            backgroundMusic.Stop();
    }
}
