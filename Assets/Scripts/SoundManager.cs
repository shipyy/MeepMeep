using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public List<SoundClip> SFX;
    public List<SoundClip> Music;
    public List<string> container_names;

    private GameObject newObj;

    public int isFirstTime;


    void Start()
    {

        for (int i = 0; i < container_names.Count; i++)
        {
            newObj = new GameObject();
            newObj.name = container_names[i];
            newObj.transform.SetParent(this.gameObject.transform);
        }

        AddSFXSources();
        AddMusicSources();

        SetVolumes();

        PlayMusic("MainMenu");
    }

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    private void AddSFXSources()
    {

        GameObject temp_container = GameObject.Find("SFX");
        foreach (SoundClip s in SFX)
        {
            s.source = temp_container.AddComponent<AudioSource>();
            
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

        }
    }
    private void AddMusicSources()
    {
        GameObject temp_container = GameObject.Find("Music");

        foreach (SoundClip s in Music)
        {
            s.source = temp_container.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void StopMusic(string name){

        SoundClip s = Music.Find(sound => sound.name == name);
        s.source.Stop();

    }

    public void PlaySFX(string name)
    {
        SoundClip s = SFX.Find(sound => sound.name == name);
        s.source.Play();
    }

    public void PlayMusic(string name)
    {
        SoundClip s = Music.Find(sound => sound.name == name);
        if(!s.source.isPlaying)
            s.source.Play();
    }

    public void setSFXVolume(float volume)
    {

        //GetComponents<AudioSource> returns an array that is why i use an array with audiosources here
        AudioSource[] sources = GameObject.Find("SFX").GetComponents<AudioSource>();

        for (int i = 0; i < sources.Length; i++)
        {
            sources[i].volume = volume;
        }

        PlayerPrefs.SetFloat("SFX_Volume", volume);

        //PlayerPrefs.Save();

    }

    public void setMusicVolume(float volume)
    {

        //GetComponents<AudioSource> returns an array that is why i use an array with audiosources here
        AudioSource[] sources = GameObject.Find("Music").GetComponents<AudioSource>();

        for (int i = 0; i < sources.Length; i++)
        {
            sources[i].volume = volume;
        }

        PlayerPrefs.SetFloat("Music_Volume", volume);

        //PlayerPrefs.Save();

    }

    public void SetVolumes()
    {

        isFirstTime = PlayerPrefs.GetInt("FirstTime");

        //IF IT IS THE FIRST TIEM BOOTING THE GAME
        if (isFirstTime == 0)
        {
            setMusicVolume(0.25f);
            setSFXVolume(0.25f);

            PlayerPrefs.SetInt("FirstTime", -1);

            //PlayerPrefs.Save();
        }
        else {
            setMusicVolume(PlayerPrefs.GetFloat("Music_Volume"));
            setSFXVolume(PlayerPrefs.GetFloat("SFX_Volume"));

            //PlayerPrefs.Save();
        }

    }

}
