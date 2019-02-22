using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagment : MonoBehaviour
{
    bool isMute = false;
    AudioSource audioS;

    private void Awake()
    {
        if (FindObjectsOfType<LevelManagment>().Length != 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
        }

        audioS = GetComponentInChildren<AudioSource>();
    }

    public void LoadLevelComplete()
    {
        SceneManager.LoadScene("LevelComplete", LoadSceneMode.Additive);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Mute()
    {
        if (isMute)
        {
            isMute = false;
            audioS.mute = false;
        }
        else
        {
            isMute = true;
            audioS.mute = true;
        }
    }
}