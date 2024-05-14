using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuButtons : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("Level_Select");
    }
    public void QuitButton()
    {
        Application.Quit();
    }

    public void PlayTutorialButton()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void PlayLevel1Button()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void PlayLevel2Button()
    {
        SceneManager.LoadScene("Level_2");
    }

    public void PlayLevel3Button()
    {
        SceneManager.LoadScene("Level_3");
    }
}
