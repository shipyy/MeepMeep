using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paused : MonoBehaviour
{

    public GameManager GM;
    public ClickManager CE;
    public void Setup(){
        gameObject.SetActive(true);
    }

    void Awake()
    {
        GM = GameManager.instance;
        CE = ClickManager.instance;
    }

    public void MainMenu(){
        CE.GoToMainMenu();
    }

    public void Continue(){
        SoundManager.instance.PlaySFX("ButtonClick");
        FindObjectOfType<MeepController>().UnPause();
    }
}
