using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    public void ButtonPlay()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    
    public void ButtonLevelSelect()
    {
        SceneManager.LoadScene(5, LoadSceneMode.Single);
    }
    
    public void ButtonExit()
    {
        Application.Quit();
    }
}
