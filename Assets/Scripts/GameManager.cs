using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private Player player;
    [SerializeField] private BoardController board;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI bestTimerText;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameOver;

    [SerializeField] private Transform[] spawnPoints;

    private float timer;
    private float bestTime = float.MaxValue;

    private bool isPlaying = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void EndGame()
    {
        isPlaying = false;

        if (timer < bestTime)
        {
            bestTime = timer;
        }

        bestTimerText.text = bestTime.ToString("F4");
        bestTimerText.gameObject.SetActive(true);

        playButton.SetActive(true);
        gameOver.SetActive(true);

        Time.timeScale = 0f;
        player.enabled = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bestTimerText.gameObject.SetActive(false);
        Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            timer += Time.deltaTime;
            timerText.text = $"{timer:F4}s";
        }
    }

    public void Pause()
    {
        isPlaying = false;
        Time.timeScale = 0f;
        player.enabled = false;

        playButton.SetActive(true);
    }

    public void Play()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }
        
        int index = Random.Range(0, spawnPoints.Length);

        timer = 0;
        timerText.text = $"{timer:F4}s";
        
        player.ResetPlayer(spawnPoints[index]);
        board.ResetBoard();

        playButton.SetActive(false);
        gameOver.SetActive(false);

        bestTimerText.gameObject.SetActive(bestTime != float.MaxValue);

        Time.timeScale = 1f;
        player.enabled = true;
        isPlaying = true;
    }
}
