using System.Collections;
using UnityEngine;
using TMPro;

public class ShoppingManager : MonoBehaviour
{
    [SerializeField] private GameObject[] instructionPages;
    [SerializeField] private GameObject player, instructionPanel, sadPlayer, happyPlayer;
    private ScoreManager scoreManager;

    private int page = 0;

    [SerializeField] private TextMeshProUGUI timerText, startTimerText, firstInsText;
    private int startTimer = 3;
    public int Timer { get; private set; } = 30;

    [SerializeField] private GameObject background;
    [SerializeField] private GameObject cam;
    public bool EarthquakeTriggered { get; set; }

    public bool GameOver { get; set; } = false;
    [SerializeField] private GameObject timesupText;
    [SerializeField] private TextMeshProUGUI[] finalScoreText;

    private SpriteRenderer timerUIRenderer;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] countdownAudio;
    [SerializeField] private AudioClip ingredientAudio;
    [SerializeField] private AudioClip bombAudio;

    private bool playCountdown;
    private PlayerController playerController;

    public static event CoroutineHandler StartShopping;

    #region Singleton

    public static ShoppingManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        scoreManager = ScoreManager.Instance;
        playerController = player.GetComponent<PlayerController>();
        timerUIRenderer = GameObject.Find("Timer").GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        StartShopping += timerCountDown;

        timerText.text = "" + Timer;
        playCountdown = true;

        playerController.enabled = false;
    }

    #region StartShoppingEvent

    private void CallStartShopping()
    {
        if (StartShopping != null)
        {
            foreach (var d in StartShopping.GetInvocationList())
            {
                StartCoroutine((d as CoroutineHandler)());
            }
        }

    }

    private void UnsubFromStartShopping()
    {
        if (StartShopping != null)
        {
            foreach (var d in StartShopping.GetInvocationList())
            {
                StartShopping -= (d as CoroutineHandler);
            }
        }
    }

    #endregion

    #region Timers

    IEnumerator StartCountdown()
    {
        while (startTimer > 0)
        {
            yield return new WaitForSeconds(1);

            startTimer--;
            startTimerText.text = "" + startTimer;
        }

        startTimerText.text = "Start!";

        yield return new WaitForSeconds(1);

        GameObject startTimerObj = GameObject.Find("StartText");
        startTimerObj.SetActive(false);
        playerController.enabled = true;
        CallStartShopping();
    }

    IEnumerator timerCountDown()
    {
        while (!GameOver && Timer > 0)
        {
            yield return new WaitForSeconds(1);

            Timer--;
            timerText.text = "" + Timer;
        }

    }

    void Last5Secs()
    {
        timerUIRenderer.color = Color.red;
        audioSource.clip = countdownAudio[0];

        if (!audioSource.isPlaying || playCountdown)
        {
            audioSource.PlayOneShot(countdownAudio[0], 0.9f);
            playCountdown = false;
        }
    }

    #endregion

    public void Instruction()
    {
        if (page < 3 && !GameOver)
        {
            instructionPages[page].SetActive(false);
            instructionPages[page + 1].SetActive(true);
            page++;
        }
        else if (!GameOver)
        {
            instructionPages[page].SetActive(false);
            instructionPanel.SetActive(false);
            page = 0;
            StartCoroutine(StartCountdown());
        }

        if (GameOver)
        {
            StartCoroutine(SceneLoader.Instance.LoadNextScene("StreetScene"));
        }
    }

    private void Update()
    {
        if (Timer <= 5 && Timer > 0)
        {
            Last5Secs();
        }

        if (Timer == 0 && !GameOver)
        {
            EndGame();
        }
    }

    // earthquake effect
    public IEnumerator Earthquake(float duration, float magnitude)
    {
        Vector3 originalPos = cam.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            cam.transform.position = new Vector2(Random.Range(-1, 1) * magnitude, originalPos.y);

            elapsed += Time.deltaTime;

            yield return null;
        }

        cam.transform.position = originalPos;
        
    }

    public void SpawnScoreSign(GameObject signPrefab)
    {
        Vector3 spawnPos = player.transform.position + new Vector3(1.5f, 0);
        Instantiate(signPrefab, spawnPos, Quaternion.identity);
    }

    public void IngredientAudio()
    {
        audioSource.PlayOneShot(ingredientAudio, 0.3f);
    }

    public void BombAudio()
    {
        audioSource.PlayOneShot(bombAudio, 1f);
    }

    void EndGame()
    {
        GameOver = true;

        if (audioSource.clip == countdownAudio[0])
        {
            audioSource.Stop();
            audioSource.clip = countdownAudio[1];
        }

        if (!audioSource.isPlaying && audioSource.clip == countdownAudio[1])
        {
            audioSource.PlayOneShot(countdownAudio[1], 0.6f);
            audioSource.clip = null;
        }

        timesupText.SetActive(true);
        playerController.enabled = false;
        scoreManager.CheckTarget();
        UnsubFromStartShopping();

        StartCoroutine(ShowFinalScore());
    }

    IEnumerator ShowFinalScore()
    {
        firstInsText.text = "1. Ingredients I got:";

        for (int i = 0; i < 4; i++)
        {
            finalScoreText[i].text = "X" + scoreManager.Scores[i];

            if (scoreManager.Scores[i] < scoreManager.ScoreTarget)
            {
                finalScoreText[i].color = Color.red;
            }
            else
            {
                finalScoreText[i].color = new Color32(13, 142, 0, 255);
            }
        }

        yield return new WaitForSeconds(3);

        if (scoreManager.TargetReached == 4)
        {
            StreetManager.goHome = 2;
            happyPlayer.SetActive(true);
        }
        else
        {
            StreetManager.goHome = 1;
            sadPlayer.SetActive(true);
        }

        instructionPages[page].SetActive(true);
        instructionPanel.SetActive(true);
    }
}
