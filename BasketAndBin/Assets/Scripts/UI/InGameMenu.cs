using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private GameObject inGameScreen;
    [SerializeField] private GameObject PauseScreen;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton1;
    [SerializeField] private Button resumeButton2;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        inGameScreen.SetActive(true);
        PauseScreen.SetActive(false);
    }

    private void OnEnable()
    {
        pauseButton.onClick.AddListener(Pause);
        resumeButton1.onClick.AddListener(Resume);
        resumeButton2.onClick.AddListener(Resume);
        quitButton.onClick.AddListener(Quit);
    }

    private void OnDisable()
    {
        pauseButton.onClick.RemoveListener(Pause);
        resumeButton1.onClick.RemoveListener(Resume);
        resumeButton2.onClick.RemoveListener(Resume);
        quitButton.onClick.RemoveListener(Quit);
    }

    private void Pause()
    {
        clickSound.Play();
        inGameScreen.SetActive(false);
        PauseScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    private void Resume()
    {
        clickSound.Play();
        inGameScreen.SetActive(true);
        PauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Quit()
    {
        clickSound.Play();
        SceneManager.LoadScene("MainMenu");
    }

}
