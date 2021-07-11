using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashCounter : MonoBehaviour
{
    public Image[] dashes;
    public int dash_remaining;

    void Start()
    {
        for (int i = 0; i < GetComponent<MeepController>().get_maxdashes_value(); i++)
            dashes[i].enabled = false;

        dash_remaining = GetComponent<MeepController>().get_dashes_value();

    }

    void Update()
    {

        dash_remaining = GetComponent<MeepController>().get_dashes_value();

        for (int i = 0; i < dash_remaining; i++)
            dashes[i].enabled = true;

        for (int i = dash_remaining; i < GetComponent<MeepController>().get_maxdashes_value(); i++)
            dashes[i].enabled = false;

    }


}
