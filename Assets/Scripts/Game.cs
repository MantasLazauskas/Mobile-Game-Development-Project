using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game : MonoBehaviour
{
    public Car car;
    public Transform startPoint;
    public TextMeshProUGUI timerText;
    public GameObject resultPanel;
    public TextMeshProUGUI finalTimeText;
    public TextMeshProUGUI bestTimeText;
    public Button restartButton;

    float timer;
    bool running;
    float best = -1f;

    void Start()
    {
        if (PlayerPrefs.HasKey("Best"))
            best = PlayerPrefs.GetFloat("Best");

        restartButton.onClick.AddListener(Restart);
        resultPanel.SetActive(false);
        StartRun();
    }

    void Update()
    {
        if (!running) return;
        timer += Time.deltaTime;
        timerText.text = timer.ToString("0.00");
    }

    // Called when car hits the Finish Line
    public void Finished()
    {
        if (!running) return;

        running = false;
        car.Stop();

        // Save best time
        if (best < 0 || timer < best)
        {
            best = timer;
            PlayerPrefs.SetFloat("Best", best);
            PlayerPrefs.Save();
        }

        finalTimeText.text = "Time: " + timer.ToString("0.00");
        bestTimeText.text = "Best: " + (best < 0 ? "--" : best.ToString("0.00"));
        resultPanel.SetActive(true);
    }

    // Called when car hits the Obstacle → just restart
    public void Crashed()
    {
        if (!running) return;
        Restart();
    }

    public void Restart()
    {
        resultPanel.SetActive(false);
        car.ResetCar(startPoint.position, startPoint.rotation);
        StartRun();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    void StartRun()
    {
        timer = 0f;
        running = true;
        timerText.text = "0.00";
    }
}