using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;


//I AM SUPOSED TO INITIALIZE THIS LEVEL MANAGER WHEN I START THE GAME 
//SO I WILL HAVE A STRUCT WITH VARIABLES FOR EVERY LEVEL
//AND THEN JUST MAKE A LIST<STRUC OF LEVEL VARIABLES>
//MAKES EVERYTHING MORE EASIER AND CENTRALIZES EVERYTHING (WHICH IS THE POINT OF A LEVEL MANAGER XD)

[System.Serializable]
public class HighscoreEntry
{
    public string name;
    public float time;
    public float CP_1;
    public float CP_2;
    public float CP_3;

    public HighscoreEntry() { }

    public HighscoreEntry(string name, float time, float cp_1, float cp_2, float cp_3)
    {
        this.name = name;
        this.time = time;
        this.CP_1 = cp_1;
        this.CP_2 = cp_2;
        this.CP_3 = cp_3;
    }

    public string getName()
    {
        return this.name;
    }

    public float getTime()
    {
        return this.time;
    }

    public float getCP_1()
    {
        return this.CP_1;
    }
    public float getCP_2()
    {
        return this.CP_2;
    }
    public float getCP_3()
    {
        return this.CP_3;
    }

    public void setCP_1(float time)
    {
        this.CP_1 = time;
    }

    public void setCP_2(float time)
    {
        this.CP_2 = time;
    }

    public void setCP_3(float time)
    {
        this.CP_3 = time;
    }

    public override string ToString()
    {
        //Debug.Log("INSIDE ENTRY TOSTRING()");
        return "NAME : " + getName() + " || TIME : " + getTime() +
                "\n" +
                "CPS" +
                "CP1 : " + getCP_1() +
                "CP2 : " + getCP_2() +
                "CP3 : " + getCP_3() + "\n";
    }

}

[System.Serializable]
public class HighscoreTable
{

    public List<HighscoreEntry> list = new List<HighscoreEntry>();

    public HighscoreTable() { }

    public HighscoreTable(List<HighscoreEntry> list)
    {
        this.list = list;
    }

    public List<HighscoreEntry> getList()
    {
        return this.list;
    }

    public override string ToString()
    {

        //Debug.Log("INSIDE HIGHSCORE TABLE TOSTRING()");

        string temp_list_string = "";

        for (int i = 0; i < this.list.Count; i++)
        {

            if (this.list[i] != null)
            {
                temp_list_string += this.list[i].ToString();
                Debug.Log(this.list[i]);
            }
            else
            {
                Debug.Log("LIST IS EMPTY");
            }
        }

        return "\nTABLE : " + temp_list_string;

    }

}

[System.Serializable]
public class DB_TABLES
{
    public List<HighscoreTable> list = new List<HighscoreTable>();
    public string saveName;
    public DB_TABLES() { }

    public override string ToString()
    {
        string temp_list_string = "";

        for (int i = 0; i < this.list.Count; i++)
        {
            if (this.list.Count != 0)
            {
                temp_list_string += this.list[i].ToString();
            }
            else
            {
                Debug.Log("BIG LIST IS EMPTY");
            }
        }

        return "NAME : " + this.saveName + "\nTABLE : " + temp_list_string;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private GameOver gameOverScreen;

    public int current_level;

    public int current_highscore;
    public int current_replay;

    private string newName;
    private float newTime;

    private DB_TABLES activeSave;
    public string savename;

    //List<List<HighscoreEntry>> Highscore_Tables = new List<List<HighscoreEntry>>();
    private List<HighscoreTable> Highscore_Tables = new List<HighscoreTable>();

    public Texture2D cursorArrow;
    private float CP_1, CP_2, CP_3;

    public void setCPs(float cp1_time, float cp2_time, float cp3_time)
    {
        CP_1 = cp1_time;
        CP_2 = cp2_time;
        CP_3 = cp3_time;
    }

    private void Start()
    {

        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);

        if (!PlayerPrefs.HasKey("1st_Boot"))
        {
            Debug.Log("FIRST TIME");
            QualitySettings.SetQualityLevel(2);
            Screen.SetResolution(Screen.resolutions[Screen.resolutions.Length - 1].width, Screen.resolutions[Screen.resolutions.Length - 1].height, true);
            Screen.fullScreen = true;

            //SET THE PREFS
            PlayerPrefs.SetInt("1st_Boot", 1);
            PlayerPrefs.SetInt("FullScreen", 1);
            PlayerPrefs.SetInt("Resolution", Screen.resolutions.Length - 1);
            PlayerPrefs.SetInt("Quality_Index", 2);
            PlayerPrefs.SetInt("SavingReplays", 0);

            //PlayerPrefs.Save();

        }
        else
        {
            Debug.Log("NOT FIRST TIME");
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality_Index"));
            Screen.SetResolution(Screen.resolutions[PlayerPrefs.GetInt("Resolution")].width, Screen.resolutions[PlayerPrefs.GetInt("Resolution")].height, false);
            Screen.fullScreen = PlayerPrefs.GetInt("FullScreen") == 1 ? true : false;

            //PlayerPrefs.Save();
        }

        activeSave = new DB_TABLES();
        activeSave.saveName = savename;

        SaveManager.Init();
        Debug.Log("savename : " + savename);

        for (int i = 0; i < 3; i++)
        {
            Highscore_Tables.Add(new HighscoreTable());
        }

        LoadTables();

        //Debug.Log("PRINTING TABLES IN AWAKE :\n");
        printtables();
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private bool checkWR(float time)
    {
        if (Highscore_Tables[current_level - 1].list.Count != 0)
        {
            Highscore_Tables[current_level - 1].list.Sort((u1, u2) => u1.getTime().CompareTo(u2.getTime()));
            float WR = Highscore_Tables[current_level - 1].list[0].getTime();

            if (newTime <= WR)
                return true;

            return false;
        }
        else
        {
            return true;
        }
    }

    public void GameOver()
    {

        newTime = FindObjectOfType<Timer>().getCurrentTime();

        if (PlayerPrefs.GetInt("SavingReplays") == 1 ?true:false)
        {
            if (checkWR(newTime))
            {
                Debug.Log("NEW WR");
                SaveManager.SaveReplay(current_level);
            }

            SaveManager.ClearTemp();
        }

        //CHECK IF IT IS TOP10
        if (checkifTop10(newTime))
        {
            Debug.Log("IS TOP 10");

            //SET CHILD VISIBLE SO I CAN GET THE SCRIPT
            GameObject.FindGameObjectWithTag("TOP10").transform.GetChild(0).gameObject.SetActive(true);

            //DELETE PLAYER
            GameObject.FindGameObjectWithTag("Player").SetActive(false);

            //SET THE SCRIPT
            gameOverScreen = GameObject.FindGameObjectWithTag("TOP10").GetComponentInChildren<GameOver>();

            //CALL THE FUNCTION THAT DISPLAYS THE UI
            gameOverScreen.Setup(newTime);

        }
        //IF IT ISNT TOP 10
        else
        {


            Debug.Log("FOUND");

            //SET CHILD VISIBLE SO I CAN GET THE SCRIPT
            GameObject.FindGameObjectWithTag("NOT_TOP10").transform.GetChild(0).gameObject.SetActive(true);

            //SET THE SCRIPT
            gameOverScreen = GameObject.FindGameObjectWithTag("NOT_TOP10").GetComponentInChildren<GameOver>();

            //DELETE PLAYER
            GameObject.FindGameObjectWithTag("Player").SetActive(false);

            //CALL THE FUNCTION THAT DISPLAYS THE UI
            gameOverScreen.Setup(newTime);



        }

    }

    public int getCurrentLevel()
    {
        return this.current_level;
    }

    public List<HighscoreTable> GetHighscoreTables()
    {
        return this.Highscore_Tables;
    }

    public HighscoreTable GetHighscoreTable_byIndex(int level)
    {
        Debug.Log("INSIDE GET HIGHSCOREBY INDEX");
        return Highscore_Tables[level];
    }

    public bool checkifTop10(float time)
    {

        Debug.Log("INSIDE CHECK TOP10");
        Debug.Log("COUNT OF TABLE AFTER TOP 10 LOG: " + Highscore_Tables.Count);
        HighscoreTable temp_table = Highscore_Tables[current_level - 1];
        temp_table.list = Highscore_Tables[current_level - 1].list;


        Debug.Log("GOT THE LIST");

        if (temp_table.list != null)
        {
            if (temp_table.list.Count < 10)
            {
                Debug.Log("FLAG 1");
                return true;
            }

            //IN CASE THERE IS A TOP 10 ALREADY
            //IM ORDERING THE TABLE BY TIME (ASCENDING)
            temp_table.list.Sort((u1, u2) => u1.getTime().CompareTo(u2.getTime()));

            //COMPARE THE PLAYER_10 TIME VS THE ONE I AM TRYING TO ADD
            if (time < temp_table.list[temp_table.list.Count - 1].getTime())
                return true;
            else
                return false;
        }

        return true;

    }

    public void AddHighscore(string name)
    {
        //SORT THE CURRENT LEVEL HIGHSCORETABLE BY TIME (ASCENDING)
        Highscore_Tables[current_level - 1].list.Sort((u1, u2) => u1.getTime().CompareTo(u2.getTime()));

        //List<HighscoreEntry> temp_list = getLevelTable();
        //temp_list.Sort((u1, u2) => u1.getTime().CompareTo(u2.getTime()));

        //REMOVE THE HIGHEST VALUE
        if (Highscore_Tables[current_level - 1].list.Count == 10)
        {
            Debug.Log("== 10");
            Highscore_Tables[current_level - 1].list.RemoveAt(Highscore_Tables[current_level - 1].list.Count - 1);
            Highscore_Tables[current_level - 1].list.Add(new HighscoreEntry(name, newTime, CP_1, CP_2, CP_3));
            //temp_list.RemoveAt(temp_list.Count - 1);
            //temp_list.Add(new HighscoreEntry(name, newTime));
        }
        else
        {
            //Debug.Log("!= 10 " + temp_list.Count);
            Debug.Log("!= 10 " + Highscore_Tables[current_level - 1].list.Count);

            Highscore_Tables[current_level - 1].list.Add(new HighscoreEntry(name, newTime, CP_1, CP_2, CP_3));
            //temp_list.Add(new HighscoreEntry(name, newTime));
        }

        Debug.Log("DID ADD");

        Debug.Log("----------------------------------");
        printtables();
        Debug.Log("----------------------------------");

        saveTables();

    }

    public void LoadTables()
    {

        ///////
        //JSON

        string db_table_json_string = SaveManager.Load(savename);

        if (db_table_json_string != null)
        {
            activeSave = JsonUtility.FromJson<DB_TABLES>(db_table_json_string);
            savename = activeSave.saveName;
            Highscore_Tables = activeSave.list;
            Debug.Log("LOADED");
        }
        ///////

    }
    public void saveTables()
    {

        /////////
        /////////
        //JSON
        activeSave.list = Highscore_Tables;

        string content = JsonUtility.ToJson(activeSave);
        SaveManager.Save(savename, content);
        Debug.Log(content);

        /////////

        Debug.Log("SAVED");

    }

    void printtables()
    {

        Debug.Log("INSIDE PRINTTABLES");

        Debug.Log("COUNT : " + Highscore_Tables.Count);

        for (int i = 0; i < Highscore_Tables.Count; i++)
        {
            Highscore_Tables[i].list.Sort((u1, u2) => u1.getTime().CompareTo(u2.getTime()));

            HighscoreTable temp_table = new HighscoreTable();
            temp_table.list = Highscore_Tables[i].getList();
            if (temp_table.list.Count != 0)
                Debug.Log("PRINTING TABLE NUMBER " + (i + 1) + "\n" + temp_table.ToString() + "\n");
            else
                Debug.Log("LIST " + (i + 1) + " IS EMPTY");
        }


    }

    public void setCurrentLevel(int level)
    {
        current_level = level;
    }
    public void setCurrentHighScoreLevel(int level)
    {
        current_highscore = level;
    }

    public int getCurrentHighScore_level()
    {
        return current_highscore;
    }

    public void setCurrentReplayLevel(int level)
    {
        current_replay = level;
    }

    public int getCurrentReplayLevel()
    {
        return current_replay;
    }

}