using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    [SerializeField] private Image[] hearts;

    private void Awake()
    {
        CacheChildHeartsIfNeeded();
    }

    public void SetLives(int lives)
    {
        CacheChildHeartsIfNeeded();

        if (hearts == null) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
            {
                hearts[i].enabled = i < lives;
            }
        }
    }

    private void CacheChildHeartsIfNeeded()
    {
        if (hearts == null || hearts.Length == 0)
        {
            hearts = GetComponentsInChildren<Image>(true);
        }
    }
}
