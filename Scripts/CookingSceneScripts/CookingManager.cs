using System.Collections;
using UnityEngine;
using TMPro;
using DigitalRuby.Tween;
using UnityEngine.UI;

public class CookingManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText, startTimerText;
    [SerializeField] private GameObject[] instructionPages;
    private int timer = 60, startTimer = 3, page = 0;
    private bool playCountdown;
    private SpriteRenderer timerUIRenderer;
    [SerializeField] private AudioClip[] countdownAudio;
    private AudioSource audioSource;
    [SerializeField] private GameObject timesupText, instructionPanel, emptyTable, giftBox, flowchart, timerObj,
        arrow, cookingInstruction, recipeCover, retryFlowchart;
    private QuestionGenerator questionGenerator;

    #region Singleton
    public static CookingManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    private void Start()
    {
        timerUIRenderer = GameObject.Find("Timer").GetComponent<SpriteRenderer>();
        questionGenerator = QuestionGenerator.Instance;
        audioSource = GetComponent<AudioSource>();
        timerText.text = "" + timer;
        timesupText.SetActive(false);
        playCountdown = true;
    }

    public void Instruction()
    {
        if (page < 4)
        {
            instructionPages[page].SetActive(false);
            instructionPages[page + 1].SetActive(true);
            page++;
        }
        else 
        {
            instructionPages[page].SetActive(false);
            instructionPanel.SetActive(false);
            page = 0;
            StartCoroutine(StartCountdown());
        }
    }

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
        StartCoroutine(TimerCountDown());
        StartCoroutine(questionGenerator.ShowQuestion(3));
    }

    private void Update()
    {
        if (timer <= 5 && timer > 0)
        {
            Last5Secs();
        }

        if (timer == 0)
        {
            EndGame();
            timer--;
        }
    }

    IEnumerator TimerCountDown()
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(1);

            timer--;
            timerText.text = "" + timer;
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

    private void EndGame()
    {
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
        EventBroker.CallGameOver();

        StartCoroutine(ToCutscene());
    }

    IEnumerator ToCutscene()
    {
        yield return new WaitForSeconds(3);

        EventBroker.UnsubscribeFromGameOver();
        emptyTable.SetActive(true);
        timesupText.SetActive(false);
        timerObj.SetActive(false);

        if (QuestionGenerator.tartNo == 0)
        {
            retryFlowchart.SetActive(true);
        }
        else
        {
            flowchart.SetActive(true);
        }

        foreach (var button in questionGenerator.buttons)
        {
            button.SetActive(false);
        }
        
        timerText.text = "";
    }

    public void SlideInGiftBox()
    {
        System.Action<ITween<Vector3>> updateGiftBoxPos = (t) =>
        {
            giftBox.transform.position = t.CurrentValue;
        };

        System.Action<ITween<Vector3>> giftBoxMoveCompleted = (t) =>
        {
            Debug.Log("move completed");
            giftBox.GetComponent<Button>().enabled = true;
        };

        Vector2 newPos = new Vector2(giftBox.transform.position.x, -3);
        giftBox.Tween("MovePalette", giftBox.transform.position, newPos, 1.5f,
            TweenScaleFunctions.CubicEaseOut, updateGiftBoxPos, giftBoxMoveCompleted);
    }

    public void ToPackingScene()
    {
        StartCoroutine(SceneLoader.Instance.LoadNextScene("PackingScene"));
    }

    public void RetryScene()
    {
        StartCoroutine(SceneLoader.Instance.LoadNextScene("CookingScene"));
    }
}
