using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance;
    private PauseMenuController _pauseMenu;
    private WinScreenController _winScreen;
    
    private void Start()
    {
        Instance = this;

        var player = GameObject.FindWithTag("Player");
        _pauseMenu = player.GetComponentInChildren<PauseMenuController>();
        _winScreen = player.GetComponentInChildren<WinScreenController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TurnOnPause();
        }
    }

    public void TurnOnPause()
    {
        _pauseMenu.TogglePauseMenu(true);
    }

    public void TurnOnWinScreen()
    {
        _winScreen.ToggleWinScreen(true);
    }
}
