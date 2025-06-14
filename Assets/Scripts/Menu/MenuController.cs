using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{

    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private GameObject confirmationPrompt = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text ControllerSenTextValue = null;
    [SerializeField] private Slider controllerSenSlider = null;
    [SerializeField] private int defaultSen = 4;
    public int mainControllerSen = 4;



    [Header("Toggle Setting")]
    [SerializeField] private Toggle invertYToggle = null;


    [Header("Graphics Setting")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 1;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropDown;
    [SerializeField] private Toggle fullScreenToggle;

    private int _qualityLevel;
    private bool _isfullScreen;
    private float _brightnessLevel;

    [Header("Levels To Load")]
    public string _newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSaveGameDialog;

    [Header("Resolution Dropdowns")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions = { };

    private void Start()
    {
        resolutions = Screen.resolutions;
        // resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        foreach (var res in Screen.resolutions)
        {
            Debug.Log(res.width + " x " + res.height);
        }

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " X " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        // resolutionDropdown.AddOptions(options);
        // resolutionDropdown.value = currentResolutionIndex;
        // resolutionDropdown.RefreshShownValue();
    }


    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }



    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    public void LoadGameDialogYes()
    {
        Debug.Log("hola mundo");
        SceneManager.LoadScene(_newGameLevel);
        // if (PlayerPrefs.HasKey("SavedLevels"))
        // {
        //     // levelToLoad = PlayerPrefs.GetString("SavedLevels");
        //     Debug.Log("hola mundo");
        //     SceneManager.LoadScene(levelToLoad);
        // }
        // else
        // {
        //     noSaveGameDialog.SetActive(true);
        // }
    }


    public void LoadGameSaved()
    {
        if (PlayerPrefs.HasKey("LastScene"))
        {
            string lastEscene = PlayerPrefs.GetString("LastScene");
            SceneManager.LoadScene(lastEscene);
        }
    }


    public void ExitButton()
    {
        Application.Quit();
    }


    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }


    public void SetControllerSen(float sensitivy)
    {
        mainControllerSen = Mathf.RoundToInt(sensitivy);
        ControllerSenTextValue.text = sensitivy.ToString("0");
    }

    public void GameplayApply()
    {
        // if (invertYToggle.isOn)
        // {
        //     PlayerPrefs.SetInt("masterInvertY", 1);

        // }
        // else
        // {
        //     PlayerPrefs.SetInt("masterInvertY", 0);
        // }

        // PlayerPrefs.SetFloat("masterSen", mainControllerSen);
        // StartCoroutine(ConfirmationBox());
    }


    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");

    }

    public void SetFullScreen(bool isFullScreen)
    {
        _isfullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);
        PlayerPrefs.SetInt("mastrFullScreen", (_isfullScreen ? 1 : 0));
        Screen.fullScreen = _isfullScreen;
        StartCoroutine(ConfirmationBox());
    }


    public void ResetButton(string menuType)
    {
        if (menuType == "Graphics")
        {
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");
            qualityDropDown.value = 1;
            QualitySettings.SetQualityLevel(1);

            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            GraphicsApply();

        }

        if (menuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }

        if (menuType == "Gameplay")
        {
            ControllerSenTextValue.text = defaultSen.ToString("0");
            controllerSenSlider.value = defaultSen;
            mainControllerSen = defaultSen;
            invertYToggle.isOn = false;
            GameplayApply();
        }

    }


    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        //Show Prompt
        StartCoroutine(ConfirmationBox());

    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
