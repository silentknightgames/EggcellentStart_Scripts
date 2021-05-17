using UnityEngine;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    [SerializeField] private GameObject ribbon, funFactPanel, backTitleButton;
    private GameObject finalGiftBox;
    private GameObject[] giftBoxes;

    private void Start()
    {
        finalGiftBox = GameObject.Find(PatternChanger.finalPattern);
        giftBoxes = GameObject.FindGameObjectsWithTag("GiftBox");

        foreach (var box in giftBoxes)
        {
            if (box != finalGiftBox)
            {
                Destroy(box);
            }
        }

        finalGiftBox.GetComponent<Image>().color = ColorChanger.finalColor;
        ribbon.GetComponent<Image>().color = ColorChanger.finalRibbon;
    }

    public void OpenPanel()
    {
        funFactPanel.SetActive(true);
    }

    public void CloseFunFact()
    {
        funFactPanel.SetActive(false);
    }

    public void BackToTitle()
    {
        StartCoroutine(SceneLoader.Instance.LoadNextScene("MainMenu"));
    }
}
