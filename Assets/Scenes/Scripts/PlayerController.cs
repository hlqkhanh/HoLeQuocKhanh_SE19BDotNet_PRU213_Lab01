using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;


    public GameObject bulletPrefab;
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;



    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4f;
    public float maxY = 4f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        rb.linearVelocity = new Vector2(moveX, moveY).normalized * moveSpeed;


        Vector3 currentPos = transform.position;

        currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);

        currentPos.y = Mathf.Clamp(currentPos.y, minY, maxY);

        transform.position = currentPos;


        if (Input.GetKey(KeyCode.Space) && Time.time > nextFireTime)
        {
            Shoot();

            nextFireTime = Time.time + fireRate; 
        }
    }

    void Shoot()
    {
        Vector3 spawnPosition = transform.position + new Vector3(0f, 0.8f, 0f); 
        

        Instantiate(bulletPrefab, spawnPosition, Quaternion.Euler(0, 0, 90));
    }
}