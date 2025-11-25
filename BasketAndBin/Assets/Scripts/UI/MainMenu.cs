using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button openSettingsButton;
    [SerializeField] private Button closeSettingsButton;

    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject settingsScreen;

    [SerializeField] private AudioSource clickSound;

    private void Start()
    {
        mainScreen.SetActive(true);
        settingsScreen.SetActive(false);
    }

    private void OnEnable()
    {
        playButton.onClick.AddListener(StartGame);
        openSettingsButton.onClick.AddListener(OpenSettings);
        closeSettingsButton.onClick.AddListener(CloseSettings);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(StartGame);
        openSettingsButton.onClick.RemoveListener(OpenSettings);
        closeSettingsButton.onClick.RemoveListener(CloseSettings);
    }

    private void StartGame()
    {
        clickSound.Play();
        SceneManager.LoadScene("MainScene");               
    }

    private void OpenSettings()
    {
        clickSound.Play();
        settingsScreen.SetActive(true);
        mainScreen.SetActive(false);
    }

    private void CloseSettings()
    {
        clickSound.Play();
        settingsScreen.SetActive(false);
        mainScreen.SetActive(true);
    }
}
