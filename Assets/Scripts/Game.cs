using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game : MonoBehaviour
{
    [Header("Car & Setup")]
    public Car car;
    public Transform startPoint;
    public TextMeshProUGUI timerText;

    [Header("Result Panel UI")]
    public GameObject resultPanel;
    public TextMeshProUGUI finalTimeText;
    public TextMeshProUGUI bestTimeText;
    public Button restartButton;

    [Header("UI Menus")]
    public GameObject pauseMenuPanel;
    public GameObject optionsPanel;
    public GameObject soundPanel;
    public GameObject hudPauseButton;

    [Header("Control Toggle UI")]
    public Button controlToggleButton;        // Drag your single toggle button here
    public TextMeshProUGUI controlButtonText; // Drag the text inside that button here
    [HideInInspector] public string controlType = "Touch"; // "Touch" or "Gyro"

    float timer;
    bool running;
    bool isPaused = false;
    float best = -1f;

    void Start()
    {
        if (PlayerPrefs.HasKey("Best"))
            best = PlayerPrefs.GetFloat("Best");

        if (PlayerPrefs.HasKey("ControlType"))
            controlType = PlayerPrefs.GetString("ControlType");

        restartButton.onClick.AddListener(Restart);

        // Ensure correct panels start hidden
        if (resultPanel != null) resultPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (soundPanel != null) soundPanel.SetActive(false);
        if (hudPauseButton != null) hudPauseButton.SetActive(true);

        UpdateControlButtonText();
        StartRun();
    }

    void Update()
    {
        if (!running || isPaused) return;
        timer += Time.deltaTime;
        timerText.text = timer.ToString("0.00");
    }

    // --- PAUSE MENU NAVIGATION ---

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (!running) return;
        isPaused = true;
        Time.timeScale = 0f; // Freezes game time and physics

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (soundPanel != null) soundPanel.SetActive(false);
        if (hudPauseButton != null) hudPauseButton.SetActive(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resumes normal time

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (soundPanel != null) soundPanel.SetActive(false);
        if (hudPauseButton != null) hudPauseButton.SetActive(true);
    }

    // --- SUB-MENU NAVIGATION ---

    public void OpenOptions()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
    }

    public void OpenSoundMenu()
    {
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (soundPanel != null) soundPanel.SetActive(true);
    }

    public void CloseSoundMenu()
    {
        if (soundPanel != null) soundPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }

    // --- SETTINGS (CONTROLS & AUDIO) ---

    public void ToggleControlType() // Hook up to your single control button in Options
    {
        if (controlType == "Touch")
        {
            controlType = "Gyro";
        }
        else
        {
            controlType = "Touch";
        }

        PlayerPrefs.SetString("ControlType", controlType);
        PlayerPrefs.Save();

        UpdateControlButtonText();
        Debug.Log("Control type toggled to: " + controlType);
    }

    void UpdateControlButtonText()
    {
        if (controlButtonText != null)
        {
            controlButtonText.text = "Control: " + controlType;
        }
    }

    public void SetVolume(float sliderValue) // Hook up to a UI Slider in the Sound Menu
    {
        AudioListener.volume = sliderValue;
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
        PlayerPrefs.Save();
    }

    // --- RACE LOGIC ---

    public void Finished()
    {
        if (!running || isPaused) return;

        running = false;
        car.Stop();

        if (best < 0 || timer < best)
        {
            best = timer;
            PlayerPrefs.SetFloat("Best", best);
            PlayerPrefs.Save();
        }

        finalTimeText.text = "Time: " + timer.ToString("0.00");
        bestTimeText.text = "Best: " + (best < 0 ? "--" : best.ToString("0.00"));

        if (resultPanel != null) resultPanel.SetActive(true);
        if (hudPauseButton != null) hudPauseButton.SetActive(false);
    }

    public void Crashed()
    {
        if (!running) return;
        Restart();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (resultPanel != null) resultPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (soundPanel != null) soundPanel.SetActive(false);
        if (hudPauseButton != null) hudPauseButton.SetActive(true);

        car.ResetCar(startPoint.position, startPoint.rotation);
        StartRun();
    }

    public void QuitToMainMenu(string menuSceneName)
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(menuSceneName);
    }

    void StartRun()
    {
        timer = 0f;
        running = true;
        timerText.text = "0.00";
    }
}