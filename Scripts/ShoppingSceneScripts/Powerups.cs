using System.Collections;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    public bool MagnetActivated { get; set; } = false;
    public bool X2Activated { get; set; } = false;

    private ShoppingManager shoppingManagerScript;
    private AudioSource audioSource;

    [SerializeField] private GameObject magnetIndicator;
    [SerializeField] private GameObject x2Indicator;
    private Vector3 magnetIndicatorOffset = new Vector3(1.5f, 1.2f);


    // Start is called before the first frame update
    void Start()
    {
        shoppingManagerScript = ShoppingManager.Instance;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        PowerupIndicators();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        // activate powerups
        if (collision.gameObject.CompareTag("Magnet") && !X2Activated)
        {
            audioSource.Play();
            Destroy(collision.gameObject);

            Debug.Log("Magnet!");
            MagnetActivated = true;
            StartCoroutine(PowerupCountDown());
        }

        if (collision.gameObject.CompareTag("X2") && !MagnetActivated)
        {
            audioSource.Play();
            Destroy(collision.gameObject);

            Debug.Log("X2!");
            X2Activated = true;
            StartCoroutine(PowerupCountDown());
        }

        if (collision.gameObject.CompareTag("Earthquake"))
        {
            audioSource.Play();
            Destroy(collision.gameObject);

            Debug.Log("Earthquake!");
            shoppingManagerScript.EarthquakeTriggered = true;
            StartCoroutine(shoppingManagerScript.Earthquake(0.25f, 0.7f));

            SpawnManager.Instance.Spawn10Objects();
        }
    }



    void PowerupIndicators()
    {
        magnetIndicator.transform.position = transform.position + magnetIndicatorOffset;
        if (MagnetActivated && !shoppingManagerScript.GameOver)
        {
            magnetIndicator.SetActive(true);
        }
        else
        {
            magnetIndicator.SetActive(false);
        }

        if (X2Activated && !shoppingManagerScript.GameOver)
        {
            x2Indicator.SetActive(true);
        }
        else
        {
            x2Indicator.SetActive(false);
        }
    }

    IEnumerator PowerupCountDown()
    {
        yield return new WaitForSeconds(7);
        MagnetActivated = false;
        X2Activated = false;

    }
}
