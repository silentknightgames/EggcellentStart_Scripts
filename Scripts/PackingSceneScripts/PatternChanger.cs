using System.Collections;
using UnityEngine;
using DigitalRuby.Tween;
using TMPro;
using UnityEngine.UI;

public class PatternChanger : MonoBehaviour
{
    [SerializeField] private GameObject colorButtons;
    [SerializeField] private TextMeshProUGUI patternText;
    public static string finalPattern;

    public void SetPattern()
    {
        foreach (var pattern in GameObject.FindGameObjectsWithTag("GiftBox"))
        {
            if (pattern != gameObject)
            {
                Destroy(pattern);
            }
        }
        gameObject.GetComponent<Button>().enabled = false;
        finalPattern = gameObject.name;

        StartCoroutine(EnableColor());

    }

    IEnumerator EnableColor()
    {
        yield return new WaitForSeconds(0.1f);

        colorButtons.SetActive(true);
        patternText.text = "Which color do you like?";

        System.Action<ITween<Vector3>> updatePalettePos = (t) =>
        {
            colorButtons.transform.position = t.CurrentValue;
        };

        System.Action<ITween<Vector3>> paletteMoveCompleted = (t) =>
        {
            Debug.Log("move completed");
        };

        Vector2 newPos = new Vector2(0, colorButtons.transform.position.y);
        colorButtons.Tween("MovePalette", colorButtons.transform.position, newPos, 1.5f,
            TweenScaleFunctions.CubicEaseOut, updatePalettePos, paletteMoveCompleted);
    }

}
