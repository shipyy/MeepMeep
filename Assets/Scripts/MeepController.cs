using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

//[RequireComponent(typeof(PlayableDirector))]
public class MeepController : MonoBehaviour
{
    //player variables
    public int maxDashes = 3;
    private int current_dashes;
    public float boost_cooldown = 3.0f;
    public float next_boost_timer = 0.0f;
    //public bool isGrounded;
    //private bool can2jump;

    //private double current_position,previous_position;
    public Rigidbody2D body;
    public float mov_x;

    public Animator player_animator;
    public Animator flag_animator;
    //public PlayableDirector director;

    public PlayerMovement player_movement;

    public GameManager GM;
    public SoundManager SM;
    public ParticleManager PM;
    public ClickManager CE;

    public bool isFinished;

    public Timer timer;

    public bool isPaused;

    public float CP_1, CP_2, CP_3;

    public Vector3 scale;

    public bool isSavingReplays;
    // Start is called before the first frame update
    void Start()
    {
        isSavingReplays = PlayerPrefs.GetInt("SavingReplays") == 1 ? true:false;

        CP_1 = CP_2 = CP_3 = 0.0f;

        isPaused = false;

        GM = GameManager.instance;
        SM = SoundManager.instance;
        PM = ParticleManager.instance;
        CE = ClickManager.instance;

        SM.PlayMusic("Level");

        Debug.Log("YUP");
        timer = FindObjectOfType<Timer>();
        body = GetComponent<Rigidbody2D>();
        player_movement = GetComponent<PlayerMovement>();
        player_animator = GetComponent<Animator>();

        Time.timeScale = 1.0f;

        isFinished = false;
        flag_animator = GameObject.FindGameObjectWithTag("Finish").GetComponent<Animator>();
        flag_animator.SetBool("isFinished", isFinished);

    }

    public void setCP(int CP_index, float time)
    {
        switch (CP_index)
        {
            case 1:
                CP_1 = time;
                break;
            case 2:
                CP_2 = time;
                break;
            case 3:
                CP_3 = time;
                break;

        }
    }

    void Update()
    {

        //transform.localScale = scale;

        //ONLY PERFORM ACTIONS IF THE GAME IS NOT PAUSED


        if(FindObjectOfType<PlayerMovement>().isStarted && !isFinished && !isPaused && isSavingReplays)
            SaveManager.ScreenShot();

        if (!isFinished && !isPaused)
        {
            player_movement.getInput();

            player_animator.SetFloat("VerticalSpeed", body.velocity.y);

            //need to use ABS because we dont want the direction to interfere (-1 for left && 1 for right)
            player_animator.SetFloat("HorizontalSpeed", Mathf.Abs(body.velocity.x));
        }

        //KEYBIND TO RESTART LEVEL IF THE PLAYER WANTS
        if (Input.GetKeyDown(KeyCode.R))
            CE.Respawn();

        if (Input.GetKeyDown(KeyCode.P) && !isFinished)
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                Time.timeScale = 0.0f;

                GameObject Paused_Object = GameObject.FindGameObjectWithTag("Paused");
                Paused_Object.transform.GetChild(0).gameObject.SetActive(true);

                Paused Paused_object_container = GameObject.FindGameObjectWithTag("Paused").GetComponentInChildren<Paused>();

                Paused_object_container.Setup();
            }
            else
            {
                UnPause();
            }

        }

        if (isPaused)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;

    }  

    public void UnPause()
    {

        Time.timeScale = 1.0f;
        GameObject.FindGameObjectWithTag("Paused").transform.GetChild(0).gameObject.SetActive(false);

        isPaused = false;

    }

    public void ChangeDash(int amount)
    {
        current_dashes = Mathf.Clamp(current_dashes + amount, 0, maxDashes);
        //Debug.Log(current_dashes + "/" + maxDashes);
    }

    public int get_dashes_value()
    {
        return current_dashes;
    }

    public int get_maxdashes_value()
    {
        return maxDashes;
    }

    public void Dash()
    {
        ChangeDash(-1);
        //Debug.Log("DASHED");
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.tag == "Enemy" || collision.collider.tag == "Water" || collision.collider.tag == "Spikes" || collision.collider.tag == "Bullet")
        {
            //Debug.Log("WATER COLLIDED");

            //MAKE PLAYER ONLY TAKE DAMAGE IF IT HASNT FINISHED
            //THIS SOLVES THE ISSUE WHERE SOMETIMES A BULLET OR ENEMY MIGHT FOLLOW THE PLAYER AFTER HE HAS CROSSED THE FINISH LINE
            //WHICH WOULD CAUSE THE PLAYER TO DIE
            //WHICH IS NOT WHAT I WANT
            if (!isFinished)
            {
                SM.PlaySFX("Death");

                PM.PlayExplosion();
            }

        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Finish"))
        {

            GM.setCPs(CP_1, CP_2, CP_3);

            timer.StopTimer();
            //Debug.Log("FINISHED");
            isFinished = true;
            flag_animator.SetBool("isFinished", isFinished);
            SoundManager.instance.PlaySFX("Finish");
        }
    }
}
