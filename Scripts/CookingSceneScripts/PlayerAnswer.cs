using System.Collections.Generic;
using UnityEngine;

public class PlayerAnswer : MonoBehaviour
{
    private readonly List<string> answerNames = new List<string>();
    private readonly List<GameObject> ingredientsOnScreen = new List<GameObject>();
    private QuestionGenerator questionGenerator;
    private Vector2 spawnPos;
    private float xPos;
    private readonly float initialXPos = 0.15f;
    private AudioSource audioSource;
    [SerializeField] private AudioClip clickAudio;

    private void Awake()
    {
        questionGenerator = QuestionGenerator.Instance;
        audioSource = GetComponent<AudioSource>();
        EventBroker.GameOver += ResetData;
        EventBroker.GameOver += StopAllCoroutines;
        xPos = initialXPos;
    }


    #region ButtonInputs
    private void Update()
    {
        spawnPos = new Vector2(xPos, 2.36f);
    }


    public void AddButter()
    {
        answerNames.Add("butter");

        ShowIngredients(0);
    }

    public void AddEgg()
    {
        answerNames.Add("egg");

        ShowIngredients(1);
    }

    public void AddFlour()
    {
        answerNames.Add("flour");

        ShowIngredients(2);
    }

    public void AddSugar()
    {
        answerNames.Add("sugar");

        ShowIngredients(3);
    }

    public void Memorized()
    {
        StopAllCoroutines();

        foreach (var ingredient in questionGenerator.IngredientsOnScreen)
        {
            ingredient.SetActive(false);
        }

        questionGenerator.EnableButtons(true);
        questionGenerator.playerDialogue.SetActive(true);
        questionGenerator.memorizedButton.SetActive(false);
        questionGenerator.tvDialogue.SetActive(false);
    }
    #endregion


    private void ShowIngredients(int index)
    {
        audioSource.PlayOneShot(clickAudio, 1);
        GameObject ingredient = Instantiate(questionGenerator.ingredientPrefabs[index],
            spawnPos, questionGenerator.ingredientPrefabs[index].transform.rotation);
        xPos += 1.2f;
        ingredientsOnScreen.Add(ingredient);

        if (answerNames.Count == QuestionGenerator.ingredientNames.Count)
        {
            Done();
        }
    }

    private void Done()
    {
        int corrCount = 0;

        for (int i = 0; i < answerNames.Count; i++)
        {

            if (answerNames[i] == QuestionGenerator.ingredientNames[i])
            {
                corrCount++;
            }

        }

        if (corrCount != answerNames.Count)
        {
            StartCoroutine(questionGenerator.NextQuestion(false));
        }
        else
        {
            StartCoroutine(questionGenerator.NextQuestion(true));
        }
    }

    public void ResetData()
    {
        foreach (var ingredient in ingredientsOnScreen)
        {
            Destroy(ingredient);
        }

        questionGenerator.playerDialogue.SetActive(false);
        ingredientsOnScreen.Clear();
        answerNames.Clear();
        xPos = initialXPos;
    }

}
