using UnityEngine;

public class InstructionsManager : MonoBehaviour
{
    public GameObject instructionsPanel;

    public void OpenInstructions()
    {
        instructionsPanel.SetActive(true); 
    }

    public void CloseInstructions()
    {
        instructionsPanel.SetActive(false); 
    }
}