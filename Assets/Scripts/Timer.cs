using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update

    public float currentTime;
    public Text currentTimeText;

    private bool isActive = false;

    void Start()
    {
        currentTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive){
            currentTime = currentTime + Time.deltaTime;
        }
        TimeSpan spantime = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = spantime.ToString(@"mm\:ss\:fff");

    }

    public void StartTimer(){

        isActive = true;
    }

    public void StopTimer(){

        isActive = false;

    }

    public float getCurrentTime(){
        return currentTime;
    }

}
