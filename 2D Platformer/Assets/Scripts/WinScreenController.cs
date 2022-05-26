using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class WinScreenController : MonoBehaviour
{
     private List<GameObject> _childrenList = new List<GameObject>();
     private List<GameObject> _playerHUD;
     
    private void Start()
    {
        _playerHUD = PlayerBehavior.Instance.GetPlayerHUD();
        foreach (Transform child in transform)
        {
            _childrenList.Add(child.gameObject);
        }

        ToggleWinScreen(false);
    }
    
    public void NextLevelButton()
    {
        var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Single);
    }

    public void RestartGameButton()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void BackToMainMenuButton()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void ToggleWinScreen(bool value)
    {
        Time.timeScale = value ?  0 : 1;
        foreach (GameObject obj in _childrenList)
        {
            obj.SetActive(value);
        }
        foreach (GameObject obj in _playerHUD)
        {
            obj.SetActive(!value);
        }
    }
}
