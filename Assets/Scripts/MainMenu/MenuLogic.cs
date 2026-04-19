using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    [SerializeField] private string introSceneName = "Intro";
    [SerializeField] private GameObject howToPanel;
    public void PlayGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetForNewRun();
        }
        SceneManager.LoadScene(introSceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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
