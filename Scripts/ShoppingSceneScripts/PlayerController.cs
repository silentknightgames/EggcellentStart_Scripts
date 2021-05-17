using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalMovement;
    private readonly float speed = 10f;
    private readonly float xRange = 5.5f;
    [SerializeField] private GameObject player;

    private Animator playerAnimator;

    
    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        HorizontalMovements();
    }



    void HorizontalMovements()
    {
        // horizontal movement
        playerAnimator.SetFloat("speed", Mathf.Abs(horizontalMovement));

        horizontalMovement = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * horizontalMovement * Time.deltaTime * speed);

        // x boundaries
        if (transform.position.x > xRange)
        {
            player.transform.position = new Vector2(xRange, transform.position.y);

        }
        if (transform.position.x < -xRange)
        {
            player.transform.position = new Vector2(-xRange, transform.position.y);
        }
    }

}
