using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickManager : MonoBehaviour
{   
    public static ClickManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    public void StartReplay(int level_index){
        GameManager.instance.setCurrentReplayLevel(level_index);
        SoundManager.instance.PlaySFX("ButtonClick");
        SoundManager.instance.StopMusic("MainMenu");
        StartScene("Replay");
    }

    public void StartLevel(int level_index){
        GameManager.instance.setCurrentLevel(level_index);
        SoundManager.instance.PlaySFX("ButtonClick");
        SoundManager.instance.StopMusic("MainMenu");
        SceneManager.LoadScene(level_index);
    }

    public void StartScene(string scene_name){
        SoundManager.instance.PlaySFX("ButtonClick");
        SceneManager.LoadScene(scene_name);
    }

    public void StartHighScores(int level){
        GameManager.instance.setCurrentHighScoreLevel(level);
        SoundManager.instance.PlaySFX("ButtonClick");
        StartScene("HighScore Table");
    }

    public void GoToMainMenu(){
        SoundManager.instance.PlaySFX("ButtonClick");
        SoundManager.instance.StopMusic("Level");
        SoundManager.instance.PlayMusic("MainMenu");
        SceneManager.LoadScene(0);
    }

    public void GoToSettings(){
        SoundManager.instance.PlaySFX("ButtonClick");
        SceneManager.LoadScene("Options");
    }

    public void Respawn(){
        SoundManager.instance.PlaySFX("ButtonClick");
        SoundManager.instance.StopMusic("Level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Exit()
    {
        SoundManager.instance.PlaySFX("ButtonClick");
        Application.Quit();
    }

    public void Continue(){
        SoundManager.instance.PlaySFX("ButtonClick");
    }

}
