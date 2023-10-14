using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class StartMenu : MonoBehaviour
{
    //btn Event
    public void FirstGame()
    {
        if(File.Exists(Application.persistentDataPath + "game_SaveData"))
        {
            File.Delete(Application.persistentDataPath + "game_SaveData");
        }

        SceneController.Instance.TransitionToNewGame();
    }

    public void ContinueGame()
    {
        SceneController.Instance.TransitionToLoadGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
