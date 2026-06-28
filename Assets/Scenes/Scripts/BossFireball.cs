using UnityEngine;

public class BossFireball : MonoBehaviour
{
    private Vector2 direction = Vector2.down;
    private float speed = 5f;
    private int damage = 20;

    public void Initialize(Vector2 moveDirection, float moveSpeed, int scoreDamage)
    {
        direction = moveDirection.normalized;
        speed = moveSpeed;
        damage = scoreDamage;

        Destroy(gameObject, 6f);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager gameManager = GameObject.FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            gameManager.DeductScore(damage);
        }

        Destroy(gameObject);
    }
}
