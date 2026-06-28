using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;

    void Start()
    {

        Destroy(gameObject, 3f);
    }

   void Update()
    {

        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
    }
}