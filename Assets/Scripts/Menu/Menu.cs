using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject menuPrincipal;
    [SerializeField] private GameObject menuOptions;

    [SerializeField] private Sprite attackMode;
    [SerializeField] private Sprite defenseMode;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private GameObject[] buttonsGameMode = new GameObject[2];

    private bool isAttackMode;
    private Resolution[] resolutions;

    private void Start()
    {
        Time.timeScale = 1f;
        isAttackMode = true;
        menuPrincipal.SetActive(true);
        menuOptions.SetActive(false);

        QualitySettings.SetQualityLevel(2);

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void Play()
    {
        if (isAttackMode) 
        { 
            SceneManager.LoadScene(1);
        } else
        {
            SceneManager.LoadScene(5);
        }
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

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(2-qualityIndex);
    }

    public void SetFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SwitchGameMode()
    {
        isAttackMode = !isAttackMode;
        if (isAttackMode)
        {
            foreach(GameObject go in buttonsGameMode)
            {
                go.GetComponent<Image>().sprite = attackMode;
            }
        } else
        {
            foreach (GameObject go in buttonsGameMode)
            {
                go.GetComponent<Image>().sprite = defenseMode;
            }
        }
    }
}
