using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private ShoppingManager shoppingManagerScript;
    private Powerups powerupsScript;

    [SerializeField] private GameObject plusTwoPrefab;
    [SerializeField] private GameObject minusTwoPrefab;
    [SerializeField] private GameObject player;

    [SerializeField] private TextMeshProUGUI[] scoreText;
    public int[] Scores { get; set; } = { 0 };
    public int ScoreTarget { get; } = 11;
    public int TargetReached { get; set; } = 0;


    #region Singleton

    public static ScoreManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        shoppingManagerScript = ShoppingManager.Instance;
        powerupsScript = GameObject.Find("Player").GetComponent<Powerups>();

        Scores = new int[4];
        InitializeScore();
    }



    void InitializeScore()
    {
        foreach (int i in Scores)
        {
            Scores[i] = 0;
        }
        foreach (TextMeshProUGUI scoreT in scoreText)
        {
            scoreT.text = "" + Scores[0];
        }

    }

    public void UpdateScore(string tag)
    {
        if (powerupsScript.X2Activated)
        {
            shoppingManagerScript.SpawnScoreSign(plusTwoPrefab);

            Debug.Log("+2 point");
            AddScoreForCatched(tag, 2);
        }
        else
        {
            Debug.Log("+1 point");
            AddScoreForCatched(tag, 1);
        }
    }

    void AddScoreForCatched(string ingredients, int addScore)
    {
        if (!shoppingManagerScript.GameOver)
        {
            if (ingredients == "flour")
            {
                Scores[0] += addScore;
                ShowAndCheckAddedScore(0);
            }
            else if (ingredients == "butter")
            {
                Scores[1] += addScore;
                ShowAndCheckAddedScore(1);
            }
            else if (ingredients == "egg")
            {
                Scores[2] += addScore;
                ShowAndCheckAddedScore(2);
            }
            else if (ingredients == "sugar")
            {
                Scores[3] += addScore;
                ShowAndCheckAddedScore(3);
            }
        }
    }

    void ShowAndCheckAddedScore(int i)
    {
        scoreText[i].text = "" + Scores[i];

        if (Scores[i] >= ScoreTarget)
        {
            scoreText[i].color = new Color32(13, 142, 0, 255);
        }
    }

    public void MinusScore(int minusScore)
    {
        shoppingManagerScript.SpawnScoreSign(minusTwoPrefab);

        if (!shoppingManagerScript.GameOver)
        {
            for (int i = 0; i < 4; i++)
            {
                Scores[i] -= minusScore;
                if (Scores[i] < 0)
                {
                    Scores[i] = 0;
                }

                if (Scores[i] < ScoreTarget)
                {
                    scoreText[i].color = Color.black;
                }

                scoreText[i].text = "" + Scores[i];
            }
        }
    }

    public void CheckTarget()
    {
        foreach (var score in Scores)
        {
            if (score >= ScoreTarget)
            {
                TargetReached++;
            }
        }

        Debug.Log(TargetReached);
    }
}
