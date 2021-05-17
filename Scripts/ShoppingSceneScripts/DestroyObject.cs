using UnityEngine;


public class DestroyObject : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D objectRb;

    private Powerups powerupsScript;
    private ShoppingManager shoppingManagerScript;
    private ScoreManager scoreManagerScript;


    public void Start()
    {
        objectRb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        powerupsScript = player.GetComponent<Powerups>();
        shoppingManagerScript = ShoppingManager.Instance;
        scoreManagerScript = ScoreManager.Instance;

    }

    public void Update()
    {
        // destroy out of bound
        if (transform.position.y < -5f)
        {
            if (gameObject.CompareTag("Bomb"))
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        // magnet effect
        if (powerupsScript.MagnetActivated)
        {
            MagnetPowerupEffect();
        }

        // destroy objects when game over
        if (shoppingManagerScript.GameOver)
        {
            Destroy(gameObject);
        }
    }

    // destroy objects when caught and add scores
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("Flour"))
        {
            shoppingManagerScript.IngredientAudio();
            gameObject.SetActive(false);
            scoreManagerScript.UpdateScore("flour");
        }
        else if (gameObject.CompareTag("Butter"))
        {
            shoppingManagerScript.IngredientAudio();
            gameObject.SetActive(false);
            scoreManagerScript.UpdateScore("butter");
        }
        else if (gameObject.CompareTag("Egg"))
        {
            shoppingManagerScript.IngredientAudio();
            gameObject.SetActive(false);
            scoreManagerScript.UpdateScore("egg");
        }
        else if (gameObject.CompareTag("Sugar"))
        {
            shoppingManagerScript.IngredientAudio();
            gameObject.SetActive(false);
            scoreManagerScript.UpdateScore("sugar");
        }
        else if (gameObject.CompareTag("Bomb"))
        {
            shoppingManagerScript.BombAudio();
            Destroy(gameObject);
            scoreManagerScript.MinusScore(2);
        }
    }



    void MagnetPowerupEffect()
    {
        Vector2 suckToPlayer = (player.transform.position - transform.position).normalized;

        if (!gameObject.CompareTag("Bomb"))
        {
            objectRb.AddForce(suckToPlayer * 15);
        }
    }
}
