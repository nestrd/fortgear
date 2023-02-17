using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour
{



    public void NewGame()
    {

        SceneManager.LoadSceneAsync(1);
        SceneManager.LoadScene(2);
    }

    public void EndlessMode()
    {
        SceneManager.LoadSceneAsync(1);
        SceneManager.LoadScene(3);

    }

    public void ShowControls()
    {

    }

    public void ReturnTitle()
    {
        SceneManager.LoadSceneAsync(1);
        SceneManager.LoadScene(0);
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }
}
