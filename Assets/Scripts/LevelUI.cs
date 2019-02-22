using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    public void MainMenuPressed()
    {
        FindObjectOfType<LevelManagment>().LoadMainMenu();
    }

    public void UndoPressed()
    {
        FindObjectOfType<GameController>().Undo();
    }
}
