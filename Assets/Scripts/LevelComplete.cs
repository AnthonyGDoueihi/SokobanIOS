using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    public void MainMenuPressed()
    {
        FindObjectOfType<LevelManagment>().LoadMainMenu();
    }

    public void NextLevelPressed()
    {
        FindObjectOfType<LevelManagment>().LoadNextLevel();
    }
}
