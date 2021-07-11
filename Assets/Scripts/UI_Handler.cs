using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Handler : MonoBehaviour
{
    public HighscoreTable table = new HighscoreTable();
    private GameManager GM;

    public GameObject rowUI;
    void Start()
    {
        GM = GameManager.instance;
        
        Setvariables();
        PopulateTable();

        /*
        Debug.Log("PRINTING TABLE FROM LEVEL" + (GM.getCurrentHighScore_level() - 1) );
        for (int i = 0; i < table.list.Count; i++)
        {
            Debug.Log(table.list[i].ToString());
        }
        */
    }

    private void Setvariables()
    {
        Debug.Log("HIGH SCORE LEVEL SELECTED IS : " + (GM.getCurrentHighScore_level()));
        table = GM.GetHighscoreTable_byIndex(GM.getCurrentHighScore_level() - 1);
        table.list.Sort((u1, u2) => u1.getTime().CompareTo(u2.getTime()));
    }

    public void PopulateTable()
    {

        int total_entries = table.list.Count;
        Debug.Log("TOTAL OF ENTRIES IS : " + total_entries);

        for (int i = 0; i < total_entries; i++)
        {
            var row = Instantiate(rowUI, transform).GetComponent<RowUI>();

            HighscoreEntry temp = table.list[i];

            switch (i)
            {
                case 0:
                    row.Rank.text = "1st";
                    row.Rank.color = Color.red;
                    break;
                case 1:
                    row.Rank.text = "2nd";
                    row.Rank.color = Color.yellow;
                    break;
                case 2:
                    row.Rank.text = "3rd";
                    row.Rank.color = Color.gray;
                    break;
            }

            if (i > 2)
                row.Rank.text = (i + 1) + "th";

            row.Name.text = temp.getName();

            TimeSpan spantime = TimeSpan.FromSeconds(temp.getTime());
            row.Time.text = spantime.ToString(@"mm\:ss\:fff");

        }

        
        for (int j = total_entries; j < 10; j++)
        {
            var row = Instantiate(rowUI, transform).GetComponent<RowUI>();

            switch (j)
            {
                case 0:
                    row.Rank.text = ("1st").ToString();
                    row.Rank.color = Color.red;
                    break;
                case 1:
                    row.Rank.text = ("2nd").ToString();
                    row.Rank.color = Color.yellow;
                    break;
                case 2:
                    row.Rank.text = ("3rd").ToString();
                    row.Rank.color = Color.gray;
                    break;
                default:
                    row.Rank.text = ((j + 1) + "th").ToString();
                    break;
            }

            row.Name.text = "N/A";
            row.Time.text = "00:00:000";

        }
    

    }

}
