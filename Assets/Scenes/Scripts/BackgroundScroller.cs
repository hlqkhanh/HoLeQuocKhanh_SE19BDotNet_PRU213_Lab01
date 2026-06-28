using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float speed = 2f;
    [SerializeField] private float overlap = 0.08f;

    private float height;
    private float spacing;
    private Vector3 startPos;
    private GameObject cloneBg;

    void Start()
    {
        startPos = transform.position;

        height = GetComponent<SpriteRenderer>().bounds.size.y;
        spacing = height - overlap;

        cloneBg = Instantiate(gameObject);

        Destroy(cloneBg.GetComponent<BackgroundScroller>());

        cloneBg.transform.position = new Vector3(startPos.x, startPos.y + spacing, startPos.z);
    }

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        cloneBg.transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < startPos.y - spacing)
        {
            transform.position = new Vector3(startPos.x, cloneBg.transform.position.y + spacing, startPos.z);
        }

        if (cloneBg.transform.position.y < startPos.y - spacing)
        {
            cloneBg.transform.position = new Vector3(startPos.x, transform.position.y + spacing, startPos.z);
        }
    }
}
