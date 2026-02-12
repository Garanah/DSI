using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsUI : MonoBehaviour
{
    //public string playSceneName = "GameScene";
    
    public GameObject optionsPanel;
    public GameObject creditPanel;

    // Bouton PLAY
    public void PlayGame()
    {
        //Debug.Log("Pas de référence scene, check script");
        SceneManager.LoadScene("LD_Phare");
    }

    // Bouton OPTIONS
    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }
    
    // Bouton CREDITS
    public void OpenCredits()
    {
        if (creditPanel.activeSelf)
        {
            CloseCredits();
        }
        else
        {
            creditPanel.SetActive(true);
        }
    }

    public void CloseCredits()
    {
        creditPanel.SetActive(false);
    }

    public void ToggleCredits()
    {
        creditPanel.SetActive(!creditPanel.activeSelf);
    }

    // Bouton QUIT
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}