using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public void PlayGame()
    {

        SceneManager.LoadScene("Gameplay");
    }


    public void ShowInstructions()
    {
        Debug.Log("Hiển thị bảng hướng dẫn!");
    }
}