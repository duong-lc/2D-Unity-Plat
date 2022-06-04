using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{
    public void ButtonTutorial()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    
    public void ButtonLevel1()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
    
    public void ButtonLevel2()
    {
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }

    public void ButtonLevelEnd()
    {
        SceneManager.LoadScene(4, LoadSceneMode.Single);
    }

    public void ButtonLevelMainMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
