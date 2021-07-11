using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{   

    public Slider Music_Slider,SFX_Slider;

    Resolution[] resolutions;

    public Dropdown resolution_dropdown,quality_dropdown;

    public Toggle fullscreen_toggle;
    public Toggle replays_toggle;

    public void Start() {
        Music_Slider.value = PlayerPrefs.GetFloat("Music_Volume");
        SFX_Slider.value = PlayerPrefs.GetFloat("SFX_Volume");

        bool isFullscreen = PlayerPrefs.GetInt("FullScreen") == 1 ? true:false;
        bool isSavingReplays = PlayerPrefs.GetInt("SavingReplays") == 1 ? true:false;

        if (fullscreen_toggle.isOn != isFullscreen)
            fullscreen_toggle.isOn = !fullscreen_toggle.isOn;

        if (replays_toggle.isOn != isSavingReplays)
            replays_toggle.isOn = !replays_toggle.isOn;

        quality_dropdown.value = PlayerPrefs.GetInt("Quality_Index");
        quality_dropdown.RefreshShownValue();

        resolutions = Screen.resolutions;
        resolution_dropdown.ClearOptions();

        List<string> options = new List<string>();

        int current_resolution_index = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " - " + resolutions[i].refreshRate + "Hz";
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                current_resolution_index = i;
        }

        resolution_dropdown.AddOptions(options);
        resolution_dropdown.value = PlayerPrefs.GetInt("Resolution");
        resolution_dropdown.RefreshShownValue();
    }

    public void SetQuality(int quality_index){
        QualitySettings.SetQualityLevel(quality_index);
        PlayerPrefs.SetInt("Quality_Index",quality_index);
    }

    public void setSFXVolume(float volume)
    {
        SoundManager.instance.setSFXVolume(volume);
    }

    public void setMusicVolume(float volume)
    {
        SoundManager.instance.setMusicVolume(volume);
    }

    public void SetFullScreen(bool isFullscreen){
        
        PlayerPrefs.SetInt("FullScreen", (isFullscreen ? 1 : 0));

        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolution_index){
        
        PlayerPrefs.SetInt("Resolution",resolution_index);
        Screen.SetResolution(resolutions[resolution_index].width,resolutions[resolution_index].height,Screen.fullScreen);
    }

    public void SetSaveReplays(bool isSavingReplays){
        PlayerPrefs.SetInt("SavingReplays", (isSavingReplays ? 1 : 0));
    }

}
