using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public GameObject starPrefab;
    public TextMeshProUGUI scoreText;

    [Header("--- LIVES ---")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] private HeartUI heartUI;

    [Header("--- ASTEROID DIFFICULTY ---")]
    [SerializeField] private float asteroidStartInterval = 2f;
    [SerializeField] private float asteroidMinInterval = 0.45f;
    [SerializeField] private float asteroidStartSpeed = 3f;
    [SerializeField] private float asteroidMaxSpeed = 8f;
    [SerializeField] private float timeToMaxDifficulty = 60f;

    [Header("--- BOSS ---")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private int bossScoreRequired = 100;
    [SerializeField] private int bossDefeatScore = 100;
    [SerializeField] private Vector3 bossSpawnPosition = new Vector3(0f, 3f, 0f);
    [SerializeField] private Sprite bossSprite;
    [SerializeField] private Sprite fireballSprite;

    private int score = 30;
    private int currentLives;
    private bool bossSpawned;
    public int Score => score;

    void Start()
    {
        LoadBossSprites();
        FindHeartUIIfNeeded();
        currentLives = maxLives;
        UpdateScoreUI();
        UpdateHeartUI();
        StartCoroutine(SpawnAsteroidRoutine());
        StartCoroutine(SpawnStarRoutine());
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
        TrySpawnBoss();
    }

    public void DeductScore(int points)
    {
        LoseLife();
    }

    public void LoseLife()
    {
        if (currentLives <= 0) return;

        currentLives--;
        UpdateHeartUI();
        TriggerCameraShake();

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        score = Mathf.Max(0, score);
        UpdateScoreUI();

        AudioSource cameraAudio = Camera.main.GetComponent<AudioSource>();
        if (cameraAudio != null)
        {
            cameraAudio.Stop();
        }

        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.Save();
        SceneManager.LoadScene("EndGame");
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void LoadBossSprites()
    {
        if (bossSprite == null)
        {
            bossSprite = LoadSpriteResource("boss");
        }

        if (fireballSprite == null)
        {
            fireballSprite = LoadSpriteResource("fireball");
        }
    }

    private void UpdateHeartUI()
    {
        if (heartUI != null)
        {
            heartUI.SetLives(currentLives);
        }
    }

    private void FindHeartUIIfNeeded()
    {
        if (heartUI == null)
        {
            heartUI = FindAnyObjectByType<HeartUI>();
        }

        if (heartUI == null)
        {
            Debug.LogWarning("HeartUI is not assigned or present in the scene. Lives UI will not be displayed.");
        }
    }

    private void TriggerCameraShake()
    {
        if (Camera.main == null) return;

        CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
        if (cameraShake != null)
        {
            cameraShake.Shake(0.2f, 0.5f);
        }
    }

    private Sprite LoadSpriteResource(string resourceName)
    {
        Sprite sprite = Resources.Load<Sprite>(resourceName);
        if (sprite != null)
        {
            return sprite;
        }

        Texture2D texture = Resources.Load<Texture2D>(resourceName);
        if (texture == null)
        {
            return null;
        }

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
    }

    private void TrySpawnBoss()
    {
        if (bossSpawned || score < bossScoreRequired) return;

        bossSpawned = true;

        GameObject bossObject = bossPrefab != null
            ? Instantiate(bossPrefab, bossSpawnPosition, Quaternion.identity)
            : new GameObject("Boss");

        bossObject.transform.position = bossSpawnPosition;

        Boss boss = bossObject.GetComponent<Boss>();
        if (boss == null)
        {
            boss = bossObject.AddComponent<Boss>();
        }

        Sprite spriteOverride = bossPrefab == null ? bossSprite : null;
        boss.Initialize(this, spriteOverride, fireballSprite);
    }

    public void OnBossDefeated()
    {
        AddScore(bossDefeatScore);
    }

    IEnumerator SpawnAsteroidRoutine()
    {
        while (true)
        {
            float difficulty = GetDifficulty();
            float randomX = Random.Range(-8f, 8f);
            GameObject asteroid = Instantiate(asteroidPrefab, new Vector3(randomX, 6f, 0f), Quaternion.identity);
            Asteroid asteroidScript = asteroid.GetComponent<Asteroid>();

            if (asteroidScript != null)
            {
                asteroidScript.fallSpeed = Mathf.Lerp(asteroidStartSpeed, asteroidMaxSpeed, difficulty);
            }

            float spawnInterval = Mathf.Lerp(asteroidStartInterval, asteroidMinInterval, difficulty);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private float GetDifficulty()
    {
        return Mathf.Clamp01(Time.timeSinceLevelLoad / timeToMaxDifficulty);
    }

    IEnumerator SpawnStarRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 5f));
            float randomX = Random.Range(-8f, 8f);
            Instantiate(starPrefab, new Vector3(randomX, 6f, 0f), Quaternion.identity);
        }
    }
}
