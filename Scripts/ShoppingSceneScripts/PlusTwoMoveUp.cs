using UnityEngine;

public class PlusTwoMoveUp : MonoBehaviour
{
    private float moveUpSpeed = 5f;

    void FixedUpdate()
    {
        transform.Translate(Vector3.up * moveUpSpeed * Time.deltaTime);
    }

    void Update()
    {
        if (transform.position.y > 0)
        {
            Destroy(gameObject);
        }
    }

}
