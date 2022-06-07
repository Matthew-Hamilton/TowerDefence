using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public AudioMixer audioMixer;

    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    List<string> resolutionList = new List<string>();

    [SerializeField] TMP_Dropdown qualityDropdown;

    [SerializeField] Slider audioSlider;

    void Start()
    {

        if (PlayerPrefs.HasKey("resolution"))
        {
            StartResolution(false);
        }
        else
            StartResolution(true);


        if (PlayerPrefs.HasKey("volume"))
        {
            Debug.Log("Saved Volume: " + PlayerPrefs.GetFloat("volume"));
            SetMasterVolume(PlayerPrefs.GetFloat("volume"));
            audioSlider.value = PlayerPrefs.GetFloat("volume");
        }

        if(PlayerPrefs.HasKey("quality"))
        {
            SetQuality(PlayerPrefs.GetInt("quality"));
            qualityDropdown.value = PlayerPrefs.GetInt("quality");
        }
    }

    void StartResolution(bool isFirst)
    {
        resolutions = Screen.resolutions;
        resolutionList.Clear();
        int currentResolutionIndex = 0;

        if (isFirst)
        {
            for (int i = 0; i < resolutions.Length; i++)
            {
                resolutionList.Add(resolutions[i].ToString());
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                    currentResolutionIndex = i;
            }
        }
        else
        {
            for (int i = 0; i < resolutions.Length; i++)
            {
                resolutionList.Add(resolutions[i].ToString());
                if (resolutions[i].ToString() == PlayerPrefs.GetString("resolution"))
                    currentResolutionIndex = i;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionList);
        resolutionDropdown.value = currentResolutionIndex;
    }

    public void SetMasterVolume(float Volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(Volume) * 20);
        PlayerPrefs.SetFloat("volume", Volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("quality", qualityIndex);
    }

    public void SetFullScreen(bool isFullscreen)
    { Screen.fullScreen = isFullscreen;}

    public void SetResolution(int resolutionID)
    {
        Screen.SetResolution(Screen.resolutions[resolutionID].width, Screen.resolutions[resolutionID].height, Screen.fullScreen);
        PlayerPrefs.SetString("resolution", Screen.resolutions[resolutionID].ToString());
    }
}
