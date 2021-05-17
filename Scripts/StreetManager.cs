using UnityEngine;
using UnityEngine.UI;

public class StreetManager : MonoBehaviour
{
    [SerializeField] private GameObject supermarketButton, mansionButton, arrow, player, homeDiag, shopDiag, shopAgainDiag;
    [SerializeField] private AudioClip shopDoorBell, doorAudio;
    private AudioSource audioSource;
    public static int goHome; // 0 = first time, 1 = shop again, 2 = go home

    #region Singleton

    public static StreetManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (goHome == 0)
        {
            EnableShopping(false);
        }
        else if (goHome == 1)
        {
            EnableShopping(true);
        }
        else
        {
            EnableHome();
        }
    }

    private void EnableButtons(string place)
    {
        if (place == "supermarket")
        {
            supermarketButton.GetComponent<Button>().enabled = true;
            mansionButton.GetComponent<Button>().enabled = false;
        }
        else if (place == "mansion")
        {
            mansionButton.GetComponent<Button>().enabled = true;
            supermarketButton.GetComponent<Button>().enabled = false;
        }
    }

    private void EnableShopping(bool again)
    {
        arrow.transform.position = new Vector2(-3.13f, 2.23f);
        arrow.transform.localRotation = Quaternion.Euler(0, 0, 90);
        arrow.SetActive(true);

        player.transform.position = new Vector2(-5.18f, -2.09f);

        EnableButtons("supermarket");

        if (again)
        {
            shopAgainDiag.SetActive(true);
        }
        else
        {
            shopDiag.SetActive(true);
        }
    }

    private void EnableHome()
    {
        arrow.transform.localRotation = Quaternion.Euler(0, 0, -90);
        arrow.transform.position = new Vector2(-4.08f, 2.23f);
        arrow.SetActive(true);

        player.transform.position = new Vector2(3f, -2.09f);

        EnableButtons("mansion");
        homeDiag.SetActive(true);
    }

    public void ToShoppingScene()
    {
        audioSource.PlayOneShot(shopDoorBell, 1f);
        arrow.SetActive(false);
        StartCoroutine(SceneLoader.Instance.LoadNextScene("ShoppingScene"));
    }

    public void ToCookingScene()
    {
        audioSource.PlayOneShot(doorAudio, 1f);
        arrow.SetActive(false);
        StartCoroutine(SceneLoader.Instance.LoadNextScene("CookingScene"));
    }

}
