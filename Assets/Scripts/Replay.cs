using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Linq;
using System.IO;

public class Replay : MonoBehaviour
{

    public Image image;
    public List<Sprite> SpriteArray;
    private int currentImage = 0;
    private float timer = 0.0f;
    public bool play;

    public int level;

    // Start is called before the first frame update
    void Start()
    {

        //AssetDatabase.Refresh();

        level = GameManager.instance.getCurrentReplayLevel();

        //SpriteArray = Resources.LoadAll("ScreenShots/LEVEL_0" + level,typeof(Sprite)).Cast<Sprite>().ToArray();
        //SpriteArray = Resources.LoadAll<Sprite>("ScreenShots/LEVEL_0" + level + "/") as Sprite[];
        //SpriteArray = Resources.LoadAll<Sprite>("ScreenShots/LEVEL_0" + level);


        List<Texture2D> textures = new List<Texture2D>();
        Texture2D texture = null;
        byte[] imageBytes;

        string folder_path = "";

        switch(level){
            case 1:
                folder_path = SaveManager.pathToYourFile_01;
                break;
            case 2:
                folder_path = SaveManager.pathToYourFile_02;
                break;
            case 3:
                folder_path = SaveManager.pathToYourFile_03;
                break;
        }

        if (Directory.Exists(folder_path))
        {
            foreach (var file in Directory.EnumerateFiles(folder_path))
            {

                Debug.Log("NAME ----- " + file);
                string imagePath = file;
                Debug.Log("FILE ----- " + imagePath);
                if (!file.Contains("meta"))
                {
                    imageBytes = File.ReadAllBytes(imagePath);
                    texture = new Texture2D(1920, 1080);
                    texture.LoadImage(imageBytes);

                    textures.Add(texture);

                }

            }

        }

        foreach (var tex in textures)
            SpriteArray.Add(Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f));

        //Debug.Log(SpriteArray.Length);
        Debug.Log(textures.Count);
        Debug.Log(SpriteArray.Count);

        if (SpriteArray.Count != 0)
        {
            Debug.Log("FILE 1 : " + SpriteArray[0]);
            image.sprite = SpriteArray[currentImage];
            GameObject.FindGameObjectWithTag("NoReplay").SetActive(false);
            play = true;
        }
        else
        {
            Debug.Log("EMPTY");
            GameObject.FindGameObjectWithTag("NoReplay").transform.GetChild(0).gameObject.SetActive(true);
        }


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            currentImage = 0;
            play = true;
            timer = Time.time;
        }
    }

    void FixedUpdate()
    {

        if (play)
        {

            if (Time.time >= timer)
            {
                currentImage++;

                if (currentImage >= SpriteArray.Count)
                {
                    GameObject.FindGameObjectWithTag("Paused").transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    image.sprite = SpriteArray[currentImage];
                    timer = Time.deltaTime;
                }

            }

        }
    }


}
