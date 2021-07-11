using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CP : MonoBehaviour
{
    public Text CP_text;

    private GameManager GM;

    private bool passed;

    public int CP_number;

    private float WR_time;

    private float cp_time;

    public void start()
    {
        passed = false;
    }

    public void Set_Time()
    {

        GM = GameManager.instance;

        Debug.Log("INDEX IS : " + (GM.getCurrentLevel()));

        if (GM.GetHighscoreTable_byIndex(GM.getCurrentLevel() - 1).list.Count != 0)
        {
            switch (CP_number)
            {
                case 1:
                    WR_time = GM.GetHighscoreTable_byIndex(GM.getCurrentLevel() - 1).list[0].getCP_1();
                    break;
                case 2:
                    WR_time = GM.GetHighscoreTable_byIndex(GM.getCurrentLevel() - 1).list[0].getCP_2();
                    break;
                case 3:
                    WR_time = GM.GetHighscoreTable_byIndex(GM.getCurrentLevel() - 1).list[0].getCP_3();
                    break;
            }

            cp_time = WR_time - cp_time;

        }
        else
        {
            WR_time = 0.0f;
        }

        Debug.Log("HERE");

        TimeSpan spantime = TimeSpan.FromSeconds(cp_time);

        if(cp_time > 0)
            CP_text.text = "CP " + CP_number + " - " + spantime.ToString(@"mm\:ss\:fff");
        else
            CP_text.text = "CP " + CP_number + " + " + spantime.ToString(@"mm\:ss\:fff");

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !passed)
        {
            cp_time = FindObjectOfType<Timer>().getCurrentTime();
            Debug.Log("cp time : " + cp_time);

            Debug.Log("LMAOLMAO");
            FindObjectOfType<MeepController>().setCP(CP_number, cp_time);
            Debug.Log(CP_number + " time : " + cp_time);

            Debug.Log(this.transform.name);
            //start showing cps UI text after the 1st one is passed only
            if(CP_number == 1)
                CP_text.gameObject.SetActive(true);
            passed = true;
            Set_Time();
        }
    }
}
