using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    [SerializeField] private string gameplaySceneName = "Gameplay";
[Header("--- ÂM THANH GAME OVER ---")]
    public AudioClip gameOverSound;
    private void Start()
    {

        if (gameOverSound != null)
        {

            AudioSource.PlayClipAtPoint(gameOverSound, Camera.main.transform.position);
        }

        int savedScore = PlayerPrefs.GetInt("FinalScore", 0);

        if (finalScoreText != null)
        {
            finalScoreText.text = "Score: " + savedScore.ToString("000");
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
{
    Debug.Log("Đã thoát game!");
    Application.Quit(); 
}
    
}
