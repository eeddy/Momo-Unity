using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public GameObject instructionPanel; 
    public GameObject setupPanel;
    
    void Start() {
         instructionPanel.SetActive(false);
         setupPanel.SetActive(false);
    }

    public void InstructionClicked() {
        instructionPanel.SetActive(true);
    }

    public void BackFromInstructionClicked() {
        instructionPanel.SetActive(false);
    }

    public void StartClicked() {
        setupPanel.SetActive(true);
    }

    public void CancelClicked() {
        setupPanel.SetActive(false);
    }

    public void ConfirmClicked() {
        SceneManager.LoadScene("MomoGame");
    }
}
