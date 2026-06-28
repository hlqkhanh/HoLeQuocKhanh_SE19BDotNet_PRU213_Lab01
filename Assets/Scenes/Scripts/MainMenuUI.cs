using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public GameObject instructionPanel;

    public void OpenInstructions()
    {
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(true);
        }
    }

    public void CloseInstructions()
    {
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(false);
        }
    }
}
