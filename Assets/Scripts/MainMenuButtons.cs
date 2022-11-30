using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{   
    //запускает главную сцену
    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    //выходит из игры
    public void ExitButton()
    {
        Application.Quit();
    }
}
