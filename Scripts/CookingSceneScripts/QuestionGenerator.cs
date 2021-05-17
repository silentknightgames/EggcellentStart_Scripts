using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestionGenerator : MonoBehaviour
{
    public GameObject[] ingredientPrefabs, buttons;
    private GameObject screenIngredient;

    private readonly List<GameObject> ingredientsToGenerate = new List<GameObject>();
    public static List<string> ingredientNames = new List<string>();
    public List<GameObject> IngredientsOnScreen { get; set; } = new List<GameObject>();

    private float ingredientsXPos;
    private readonly float initialXPos = -7.5f;
    private int ingredientNum = 3;
    public static int tartNo = 0;

    private PlayerAnswer playerAnswer;
    [SerializeField] private GameObject cryingPlayer, happyPlayer;
    public GameObject memorizedButton, playerDialogue, tvDialogue;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private AudioClip corrAudio, wrongAudio;
    private AudioSource audioSource;

    #region Singleton

    public static QuestionGenerator Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    private void Start()
    {
        playerAnswer = GetComponent<PlayerAnswer>();
        audioSource = GetComponent<AudioSource>();

        EnableButtons(false);
        cryingPlayer.SetActive(false);
        happyPlayer.SetActive(false);

        ingredientsXPos = initialXPos;
        scoreText.text = "X" + tartNo;

        EventBroker.GameOver += ResetData;
        EventBroker.GameOver += StopAllCoroutines;
    }

    public void EnableButtons(bool enable)
    {
        foreach (var button in buttons)
        {
            if (enable == false)
            {
                button.GetComponent<Button>().enabled = false;
                button.GetComponent<Image>().color = new Color32(192, 192, 192, 255);
            }
            else
            {
                button.GetComponent<Button>().enabled = true;
                button.GetComponent<Image>().color = Color.white;
            }
        }
    }

    public IEnumerator ShowQuestion(int num)
    {
        memorizedButton.SetActive(true);
        tvDialogue.SetActive(true);

        for (int i = 0; i < num; i++)
        {
            int index = Random.Range(0, ingredientPrefabs.Length);
            ingredientsToGenerate.Add(ingredientPrefabs[index]);
            AddName(index);
        }

        foreach (var ingredient in ingredientsToGenerate)
        {
            Vector2 spawnPos = new Vector2(ingredientsXPos, 3.8f);
            screenIngredient = Instantiate(ingredient, spawnPos, ingredient.transform.rotation);
            IngredientsOnScreen.Add(screenIngredient);

            ingredientsXPos += 1.2f;
        }

        yield return new WaitForSeconds(num - 2);

        foreach (var ingredient in IngredientsOnScreen)
        {
            ingredient.SetActive(false);
        }

        EnableButtons(true);
        memorizedButton.SetActive(false);
        tvDialogue.SetActive(false);
        playerDialogue.SetActive(true);
    }

    private void AddName(int index)
    {
        string name = index == 0 ? "butter" : index == 1 ? "egg" : index == 2 ? "flour" : "sugar";
        ingredientNames.Add(name);
    }

    public IEnumerator NextQuestion(bool corr)
    {
        EnableButtons(false);
        tvDialogue.SetActive(true);

        foreach (var ingredient in IngredientsOnScreen)
        {
            ingredient.SetActive(true);
        }

        if(corr)
        {
            Debug.Log("Correct");
            happyPlayer.SetActive(true);
            audioSource.PlayOneShot(corrAudio, 1);

            ingredientNum++;

            if (tartNo < 9)
            {
                tartNo++;
                scoreText.text = "X" + tartNo;
            }
        }
        else
        {
            Debug.Log("Wrong");
            cryingPlayer.SetActive(true);
            audioSource.PlayOneShot(wrongAudio, 1);

            if (ingredientNum > 3)
            {
                ingredientNum--;
            }
        }

        ingredientsXPos = initialXPos;

        yield return new WaitForSeconds(1);

        playerAnswer.ResetData();
        ResetData();
        StartCoroutine(ShowQuestion(ingredientNum));
    }

    private void ResetData()
    {
        cryingPlayer.SetActive(false);
        happyPlayer.SetActive(false);
        EnableButtons(false);
        tvDialogue.SetActive(false);
        memorizedButton.SetActive(false);

        foreach (var ingredient in IngredientsOnScreen)
        {
            Destroy(ingredient);
        }

        ingredientsToGenerate.Clear();
        IngredientsOnScreen.Clear();
        ingredientNames.Clear();

        if (ingredientNum > 6)
        {
            ingredientNum = 6;
        }
    }

}