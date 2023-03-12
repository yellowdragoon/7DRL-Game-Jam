using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour { 

    [SerializeField] private string newGameLevel = "level1";
    [SerializeField] private string mainMenuScreen = "MainMenu";
    public void newGameButton()
    {
        SceneManager.LoadScene(newGameLevel);
    }

    public void mainMenuButton()
    {
        SceneManager.LoadScene(mainMenuScreen);
    }
}
