using UnityEngine;
using UnityEngine.SceneManagement;

public class Asteroid : MonoBehaviour
{
    [Header("--- CÀI ĐẶT TỐC ĐỘ ---")]
    public float fallSpeed = 3f;
    public float rotateSpeed = 50f;

    [Header("--- CÀI ĐẶT ÂM THANH ---")]
    public AudioClip crashSound;

    private void TriggerCameraShake()
    {
        if (Camera.main == null) return;

        CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
        if (cameraShake != null)
        {
            cameraShake.Shake(0.2f, 0.5f);
        }
    }

    void Start()
    {
        Destroy(gameObject, 10f);
    }

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (crashSound != null)
            {
                AudioSource.PlayClipAtPoint(crashSound, Camera.main.transform.position);
            }

            TriggerCameraShake();

            GameManager gameManager = GameObject.FindFirstObjectByType<GameManager>();
            if (gameManager != null)
            {
                gameManager.DeductScore(50);
            }

            Destroy(gameObject);
        }
   
        if (other.CompareTag("Laser"))
        {
            if (crashSound != null)
            {
                AudioSource.PlayClipAtPoint(crashSound, Camera.main.transform.position);
            }

            TriggerCameraShake();
        
            Destroy(other.gameObject);
            Destroy(gameObject);
        }      
    }
}