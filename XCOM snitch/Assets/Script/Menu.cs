using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public void StartGame()
    {
        Application.LoadLevel("GameScene");
    }

    public void Settings()
    {
        //no one
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
