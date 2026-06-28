using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("--- HEALTH ---")]
    [SerializeField] private int maxHealth = 20;

    [Header("--- ATTACK ---")]
    [SerializeField] private float fireRate = 1.2f;
    [SerializeField] private float fireballSpeed = 5f;
    [SerializeField] private int fireballDamage = 20;

    [Header("--- MOVEMENT ---")]
    [SerializeField] private float moveRange = 3f;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float bossScale = 0.12f;
    [SerializeField] private float fireballScale = 0.18f;

    private int currentHealth;
    private float nextFireTime;
    private float startX;
    private Sprite fireballSprite;
    private GameManager gameManager;
    private Transform player;
    private Transform healthFill;
    private float healthBarWidth = 20f;
    private float healthBarY = 13f;

    public void Initialize(GameManager manager, Sprite bossSprite, Sprite fireball)
    {
        gameManager = manager;
        fireballSprite = fireball;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        bool hadSprite = spriteRenderer != null && spriteRenderer.sprite != null;
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        if (bossSprite != null)
        {
            spriteRenderer.sprite = bossSprite;
        }

        spriteRenderer.sortingOrder = 5;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider2D>();
        }

        collider.isTrigger = true;
        if (spriteRenderer.sprite != null)
        {
            collider.size = spriteRenderer.sprite.bounds.size;
            healthBarWidth = spriteRenderer.sprite.bounds.size.x * 0.8f;
            healthBarY = spriteRenderer.sprite.bounds.extents.y + 0.8f;
        }

        if (!hadSprite)
        {
            transform.localScale = Vector3.one * bossScale;
        }

        currentHealth = maxHealth;
        startX = transform.position.x;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        CreateHealthBar();
        UpdateHealthBar();
    }

    void Update()
    {
        MoveSideToSide();

        if (Time.time >= nextFireTime)
        {
            ShootFireball();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void MoveSideToSide()
    {
        float x = startX + Mathf.Sin(Time.time * moveSpeed) * moveRange;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    private void ShootFireball()
    {
        GameObject fireball = new GameObject("BossFireball");
        fireball.transform.position = transform.position + Vector3.down * 1.1f;

        SpriteRenderer renderer = fireball.AddComponent<SpriteRenderer>();
        renderer.sprite = fireballSprite;
        renderer.sortingOrder = 6;
        fireball.transform.localScale = Vector3.one * fireballScale;

        CircleCollider2D collider = fireball.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;

        Rigidbody2D rb = fireball.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        BossFireball fireballScript = fireball.AddComponent<BossFireball>();
        Vector2 direction = Vector2.down;

        if (player != null)
        {
            direction = (player.position - fireball.transform.position).normalized;
        }

        fireballScript.Initialize(direction, fireballSpeed, fireballDamage);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Laser")) return;

        TakeDamage(1);
        Destroy(other.gameObject);
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            if (gameManager != null)
            {
                gameManager.OnBossDefeated();
            }

            Destroy(gameObject);
        }
    }

    private void CreateHealthBar()
    {
        Sprite barSprite = CreateBarSprite();

        GameObject background = new GameObject("BossHealthBackground");
        background.transform.SetParent(transform);
        background.transform.localPosition = new Vector3(0f, healthBarY, 0f);
        background.transform.localScale = new Vector3(healthBarWidth, 0.5f, 1f);

        SpriteRenderer backgroundRenderer = background.AddComponent<SpriteRenderer>();
        backgroundRenderer.sprite = barSprite;
        backgroundRenderer.color = new Color(0.15f, 0.15f, 0.15f, 1f);
        backgroundRenderer.sortingOrder = 7;

        GameObject fill = new GameObject("BossHealthFill");
        fill.transform.SetParent(transform);
        fill.transform.localPosition = new Vector3(0f, healthBarY, -0.01f);
        fill.transform.localScale = new Vector3(healthBarWidth, 0.35f, 1f);

        SpriteRenderer fillRenderer = fill.AddComponent<SpriteRenderer>();
        fillRenderer.sprite = barSprite;
        fillRenderer.color = Color.red;
        fillRenderer.sortingOrder = 8;

        healthFill = fill.transform;
    }

    private void UpdateHealthBar()
    {
        if (healthFill == null) return;

        float percent = Mathf.Clamp01((float)currentHealth / maxHealth);
        float width = healthBarWidth * percent;
        healthFill.localScale = new Vector3(width, 0.35f, 1f);
        healthFill.localPosition = new Vector3(-(healthBarWidth - width) * 0.5f, healthBarY, -0.01f);
    }

    private Sprite CreateBarSprite()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
    }
}
