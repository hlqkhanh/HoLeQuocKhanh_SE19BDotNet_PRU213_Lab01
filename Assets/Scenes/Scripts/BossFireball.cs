using UnityEngine;

public class BossFireball : MonoBehaviour
{
    private Vector2 direction = Vector2.down;
    private float speed = 5f;
    private float rotateSpeed = 360f;

    public void Initialize(Vector2 moveDirection, float moveSpeed)
    {
        direction = moveDirection.normalized;
        speed = moveSpeed;

        Destroy(gameObject, 6f);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager gameManager = GameObject.FindAnyObjectByType<GameManager>();
        if (gameManager != null)
        {
            gameManager.LoseLife();
        }

        Destroy(gameObject);
    }
}
