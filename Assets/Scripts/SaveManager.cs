using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public static class SaveManager
{

    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
    public static readonly string pathToYourFile = Application.dataPath + "/Resources/ScreenShots/Temp";
    public static readonly string pathToYourFile_01 = Application.dataPath + "/Resources/ScreenShots/LEVEL_01";
    public static readonly string pathToYourFile_02 = Application.dataPath + "/Resources/ScreenShots/LEVEL_02";
    public static readonly string pathToYourFile_03 = Application.dataPath + "/Resources/ScreenShots/LEVEL_03";
    //this is the name of the file
    static string fileName = "/frame_";
    //this is the file type
    static string fileType = ".png";

    static private int CurrentScreenshot { get => PlayerPrefs.GetInt("ScreenShot"); set => PlayerPrefs.SetInt("ScreenShot", value); }

    public static void ClearTemp()
    {

        string[] fileEntries = Directory.GetFiles(pathToYourFile);

        foreach (string fileName in fileEntries)
            File.Delete(fileName);


        if (!Directory.Exists(pathToYourFile))
            Directory.CreateDirectory(pathToYourFile);

        Debug.Log("Cleared TEMP");
    }

    public static void ClearLevel(int index_level)
    {
        Debug.Log("INSIDE CLEAR LEVEL");

        string[] fileEntries;

        switch (index_level)
        {
            case 1:
                fileEntries = Directory.GetFiles(pathToYourFile_01);

                foreach (string fileName in fileEntries)
                    File.Delete(fileName);
                break;
            case 2:
                fileEntries = Directory.GetFiles(pathToYourFile_02);

                foreach (string fileName in fileEntries)
                    File.Delete(fileName);
                break;
            case 3:
                fileEntries = Directory.GetFiles(pathToYourFile_03);

                foreach (string fileName in fileEntries)
                    File.Delete(fileName);
                break;

        }
    }
    private static void Replace(string path)
    {
        Debug.Log("REPLACING NOW");

        foreach (var file in Directory.EnumerateFiles(pathToYourFile))
        {
            string destFile = Path.Combine(path, Path.GetFileName(file));

            Debug.Log("NAME ----- " + destFile);

            if (!File.Exists(destFile))
                File.Move(file, destFile);

        }

        ClearTemp();
    }

    public static void SaveReplay(int level)
    {
        switch (level)
        {
            case 1:
                ClearLevel(1);
                Replace(pathToYourFile_01);
                break;
            case 2:
                ClearLevel(2);
                Replace(pathToYourFile_02);
                break;
            case 3:
                ClearLevel(3);
                Replace(pathToYourFile_03);
                break;

        }

    }

    public static void ScreenShot()
    {
        UnityEngine.ScreenCapture.CaptureScreenshot(pathToYourFile + fileName + CurrentScreenshot + fileType);
        CurrentScreenshot++;
    }

    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
            Directory.CreateDirectory(SAVE_FOLDER);

        if (!Directory.Exists(pathToYourFile))
            Directory.CreateDirectory(pathToYourFile);

        if (!Directory.Exists(pathToYourFile_01))
            Directory.CreateDirectory(pathToYourFile_01);

        if (!Directory.Exists(pathToYourFile_02))
            Directory.CreateDirectory(pathToYourFile_02);

        if (!Directory.Exists(pathToYourFile_03))
            Directory.CreateDirectory(pathToYourFile_03);

    }

    public static void Save(string filename, string content)
    {
        File.WriteAllText(SAVE_FOLDER + filename + ".json", content);
    }

    public static string Load(string filename)
    {
        if (File.Exists(SAVE_FOLDER + filename + ".json"))
            return File.ReadAllText(SAVE_FOLDER + filename + ".json");
        else
            return null;
    }

}
