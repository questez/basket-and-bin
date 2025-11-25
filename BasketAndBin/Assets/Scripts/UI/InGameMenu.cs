using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class InGameMenu : MonoBehaviour
{
    public static bool PauseMode { get; private set; }

    [SerializeField] private AudioSource clickSound;

    [SerializeField] private GameObject inGameScreen;
    [SerializeField] private GameObject PauseScreen;

    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton1;
    [SerializeField] private Button resumeButton2;
    [SerializeField] private Button quitButton;

    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        PauseMode = false;
    }

    private void Start()
    {
        inGameScreen.SetActive(true);
        PauseScreen.SetActive(false);        
        Player.ResetScore();
        Time.timeScale = 1f;
    }

    private void Update()
    {
        scoreText.text = "SCORE: " + Player.GetScore().ToString();
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

    private void Resume()
    {
        PauseMode = false;
        clickSound.Play();
        inGameScreen.SetActive(true);
        PauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Pause()
    {
        PauseMode = true;
        clickSound.Play();
        inGameScreen.SetActive(false);
        PauseScreen.SetActive(true);
        Time.timeScale = 0f;
    }    

    private void Quit()
    {
        clickSound.Play();
        SceneManager.LoadScene("MainMenu");              
    }
}
