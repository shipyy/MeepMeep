using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{   
    public Text time_text;
    public InputField newName;
    public GameManager GM;
    public ClickManager CE;

    public void Awake() {
        GM = GameManager.instance;
        CE = ClickManager.instance;
    }

    public void Setup(float time){
        gameObject.SetActive(true);
        time_text.text = time.ToString();

        TimeSpan spantime = TimeSpan.FromSeconds(time);
        time_text.text = spantime.ToString(@"mm\:ss\:fff");
    }

    public void Submit(){
        GM.AddHighscore(newName.text.ToUpper());
        CE.GoToMainMenu();
    }

    public void Restart(){
        Debug.Log("Restart");
        CE.Respawn();
    }

    public void MainMenu(){
        CE.GoToMainMenu();
    }


}   
