using UnityEngine;

public class Star : MonoBehaviour
{
    [Header("--- CÀI ĐẶT TỐC ĐỘ ---")]
    public float fallSpeed = 3f;

    [Header("--- CÀI ĐẶT ÂM THANH ---")]
    public AudioClip collectSound;

    void Update()
    {

        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        

        if (transform.position.y < -6f) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {

            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, Camera.main.transform.position);
            }


            GameManager gm = GameObject.FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.AddScore(10);
            }


            Destroy(gameObject);
        }
    }
}