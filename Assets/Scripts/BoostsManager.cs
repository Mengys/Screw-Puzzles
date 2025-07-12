using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BoostsManager : MonoBehaviour
{
    [SerializeField] private GameObject magnet;
    [SerializeField] private GameObject magnetText;
    [SerializeField] private GameObject broomstick;
    [SerializeField] private GameObject broomstickText;
    [SerializeField] private GameObject holesManager;
    [SerializeField] private GameObject _tutorialMagnet;
    [SerializeField] private GameObject _tutorialBroomstick;
    [SerializeField] private GameObject _magnetLock;
    [SerializeField] private GameObject _broomstickLock;
    [SerializeField] private GameObject _broomstickEffect;

    [SerializeField] private int magnetCount = 3;
    [SerializeField] private int broomstickCount = 3;

    private void Start() {
        magnetCount = PlayerPrefs.GetInt("Magnet");
        broomstickCount = PlayerPrefs.GetInt("Broomstick");
        magnetText.GetComponent<TextMeshProUGUI>().text = magnetCount.ToString();
        broomstickText.GetComponent<TextMeshProUGUI>().text = broomstickCount.ToString();

        var level = PlayerPrefs.GetInt("CurrentLevel");
        if (level >= 2) {
            _magnetLock.SetActive(false);
        }
        if (level >= 3) {
            _broomstickLock.SetActive(false);
        }
    }

    public void UseMagnet() {
        if (magnetCount == 0) return;
        _tutorialMagnet.SetActive(false);
        magnetCount--;
        PlayerPrefs.SetInt("Magnet", magnetCount);
        magnetText.GetComponent<TextMeshProUGUI>().text = magnetCount.ToString();
        FindFirstObjectByType<ParentBolt>().UseMagnet();
    }

    public void UseBroomstick() {
        if (broomstickCount == 0) return;
        _tutorialBroomstick.GetComponent<TutorialBroomstick>().SendBoltsToBox();
        _tutorialBroomstick.GetComponent<TutorialBroomstick>().Deactivate();
        if (holesManager.GetComponent<HolesManager>().FreeHolesCount() == 5) return;
        broomstickCount--;
        _broomstickEffect.SetActive(true);
        PlayerPrefs.SetInt("Broomstick", broomstickCount);
        broomstickText.GetComponent<TextMeshProUGUI>().text = broomstickCount.ToString();
        holesManager.GetComponent<HolesManager>().SendBoltsToBoltBox();
    }

    public void UseBroomstickForCoins(int coins) {
        var gameSettings = FindAnyObjectByType<GameSettings>();
        if (gameSettings.CurrentMoney < coins) return;
        gameSettings.AddMoney(-coins);
        broomstickCount++;
        UseBroomstick();
        FindAnyObjectByType<Restart>().gameObject.SetActive(false);
        broomstickText.GetComponent<TextMeshProUGUI>().text = broomstickCount.ToString();
        holesManager.GetComponent<HolesManager>().SendBoltsToBoltBox();
    }

    public void UseBroomstickForAd() {
        broomstickCount++;
        UseBroomstick();
        FindAnyObjectByType<Restart>().gameObject.SetActive(false);
        broomstickText.GetComponent<TextMeshProUGUI>().text = broomstickCount.ToString();
        holesManager.GetComponent<HolesManager>().SendBoltsToBoltBox();
    }

    public void AddMagnet(int i) {
        magnetCount++;
        PlayerPrefs.SetInt("Magnet", magnetCount);
        magnetText.GetComponent<TextMeshProUGUI>().text = magnetCount.ToString();
    }

    public void AddBroomstick(int i) {
        broomstickCount++;
        PlayerPrefs.SetInt("Broomstick", broomstickCount);
        broomstickText.GetComponent<TextMeshProUGUI>().text = broomstickCount.ToString();
    }

    public void RemoveBroomstick(int i) {
        broomstickCount--;
        PlayerPrefs.SetInt("Broomstick", broomstickCount);
        broomstickText.GetComponent<TextMeshProUGUI>().text = broomstickCount.ToString();
    }

    public void StartTutorialMagnet() {
        _magnetLock.SetActive(false);
        if (PlayerPrefs.GetInt("TutorialMagnetCompleted") == 1) return;
        //if (PlayerPrefs.GetInt("CurrentLevel") != 2) return;
        AddMagnet(1);
        _tutorialMagnet.SetActive(true);
        PlayerPrefs.SetInt("TutorialMagnetCompleted", 1);
    }

    public void StartTutorialBroomstick() {
        _broomstickLock.SetActive(false);
        if (PlayerPrefs.GetInt("TutorialBroomstickCompleted") == 1) return;
        //if (PlayerPrefs.GetInt("CurrentLevel") != 3) return;

        AddBroomstick(1);
        _tutorialBroomstick.SetActive(true);
        PlayerPrefs.SetInt("TutorialBroomstickCompleted", 1);
    }
}
