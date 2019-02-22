using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LevelPressed(string level)
    {
        FindObjectOfType<LevelManagment>().LoadLevel(level);
    }


}
