using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject menuPrincipal;
    [SerializeField] private GameObject menuOptions;

    private void Start()
    {
        menuPrincipal.SetActive(true);
        menuOptions.SetActive(false);
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Options()
    {
        Debug.Log("Click On Options");
        menuPrincipal.SetActive(false);
        menuOptions.SetActive(true);
    }

    public void Back()
    {
        Debug.Log("Click On Options");
        menuOptions.SetActive(false);
        menuPrincipal.SetActive(true);
    }

    public void Quit()
    {
        Debug.Log("Quit the Game");
        Application.Quit();
    }
}
