using UnityEngine;

public class ShieldEffect : MonoBehaviour
{
    public float blinkSpeed = 10f;
    
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
        

        Destroy(gameObject, 2f);
    }

    void Update()
    {


        float blinkAlpha = Mathf.PingPong(Time.time * blinkSpeed, originalColor.a);
        

        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, blinkAlpha);
    }
}