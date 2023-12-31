using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Options()
    {
        Debug.Log("Click On Options");
    }

    public void Quit()
    {
        Debug.Log("Quit the Game");
        Application.Quit();
    }
}
