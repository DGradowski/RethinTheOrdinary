using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    public Button[] buttons;

    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenLevel(int levelId)
    {
        string levelName = "Level " + levelId;
        if (SceneExists(levelName))
        {
            SceneManager.LoadScene(levelName);
        }else
        {
            Debug.LogWarning($"\"{levelName}\" is not available");
        }
        
    }

    // Funkcja sprawdzaj¹ca czy scena o danej nazwie istnieje 
    private bool SceneExists(string sceneName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = Path.GetFileNameWithoutExtension(path);

            if (name == sceneName)
            {
                return true;
            }
        }

        return false;
    }
}
