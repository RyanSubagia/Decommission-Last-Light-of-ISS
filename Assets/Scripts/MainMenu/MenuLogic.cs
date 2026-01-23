using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    [SerializeField] private string introSceneName = "Intro";
    [SerializeField] private GameObject howToPanel;
    public void PlayGame()
    {
        SceneManager.LoadScene(introSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenHowTo()
    {
        howToPanel.SetActive(true);
    }

    public void CloseHowTo()
    {
        howToPanel.SetActive(false);
    }
}
