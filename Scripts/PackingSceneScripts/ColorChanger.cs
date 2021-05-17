using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.Tween;

public class ColorChanger : MonoBehaviour
{
    private Image colorChosen;
    [SerializeField] private GameObject ribbon;
    private static bool finishChoosing, finishRibbon;
    public static Color32 finalColor, finalRibbon;
    private GameObject giftBox, colorButtons, palette;
    private PackingManager packingManager;

    private void Awake()
    {
        finishChoosing = false;
        finishRibbon = false;
        colorChosen = GetComponent<Image>();
        giftBox = GameObject.FindGameObjectWithTag("GiftBox");
        colorButtons = GameObject.Find("PatternColor");
        palette = GameObject.Find("ColorButtons");
        packingManager = GameObject.Find("PackingManager").GetComponent<PackingManager>();
    }

    public void ChangeColor()
    {        
        if (!finishChoosing)
        {
            giftBox.GetComponent<Image>().color = colorChosen.color;
        }

        if (!finishRibbon)
        {
            if (ribbon != null)
            {
                ribbon.GetComponent<Image>().color = colorChosen.color;
            }
        }
    }

    public void SetColor()
    {
        System.Action<ITween<Vector3>> updateButtonsPos = (t) =>
        {
            colorButtons.transform.position = t.CurrentValue;
        };

        System.Action<ITween<Vector3>> buttonsMoveCompleted = (t) =>
        {
            Debug.Log("move completed");
            colorButtons.transform.position = new Vector3(15, 0);
        };

        System.Action<ITween<Vector3>> updatePalettePos = (t) =>
        {
            palette.transform.position = t.CurrentValue;
        };

        System.Action<ITween<Vector3>> paletteMoveCompleted = (t) =>
        {
            Debug.Log("move completed");
            palette.transform.position = new Vector3(15, 0);
        };

        if (finishChoosing)
        {
            finishRibbon = true;
        }

        finishChoosing = true;
        finalColor = giftBox.GetComponent<Image>().color;
        finalRibbon = ribbon.GetComponent<Image>().color;

        if (!finishRibbon)
        {
            packingManager.EnableDragTarts();

            Vector3 offset = new Vector3(-15, 0);
            colorButtons.Tween("MovePalette", colorButtons.transform.position, colorButtons.transform.position + offset, 1.5f,
                TweenScaleFunctions.CubicEaseOut, updateButtonsPos, buttonsMoveCompleted);
        }
        else
        {
            Vector3 offset = new Vector3(-15, 0);
            palette.Tween("MovePalette", palette.transform.position, palette.transform.position + offset, 1.5f,
                TweenScaleFunctions.CubicEaseOut, updatePalettePos, paletteMoveCompleted);

            packingManager.FinishRibbon();
        }
        
    }


}
