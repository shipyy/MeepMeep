using System.Collections;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [Header("Properties")]
    public Animator player_animator;
    public MeepController player_controller;
    public Rigidbody2D body;
    public Transform FeetCheck;
    public LayerMask groundMask;
    public LayerMask wallMask;

    [Header("Walk")]
    public bool isGrounded;
    public bool isFeetTouching;
    public float speed;
    public float mov_x;

    [Header("Jump")]
    public bool clickedS = false;
    public int jump_count;
    public int max_jumps;
    public float jump_force = 6.0f;
    public float quickFallRate = 20.0f;

    [Header("Dash")]
    //DASH VARIABLES
    public bool isDashing;
    public float DashSpeed = 0.1f; // time that the player is dashing
    public float DashDistance = 25f;

    [Header("Wall Jump")]
    public float WallJumpTime = 0.05f;
    public float WallSlideSpeed = 5f;
    public float WallDistance;
    public float JumpTime;
    public bool isWallSliding;
    public RaycastHit2D WallCheckHit;

    public Timer timer;
    public bool isStarted;

    void Start()
    {
        timer = FindObjectOfType<Timer>();
        isStarted = false;
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            body.velocity = new Vector2(mov_x * speed, body.velocity.y);
            //transform.Translate(Vector2.right * mov_x * speed * Time.deltaTime);
            //body.AddForce(new Vector2(mov_x * speed, body.velocity.y));
        }

    }

    public void getInput()
    {

        mov_x = Input.GetAxisRaw("Horizontal");

        if (mov_x != 0 && !isStarted)
        {
            timer.StartTimer();
            isStarted = true;
        }
        //moveDir = new Vector2(mov_x, body.velocity.y);
        //body.velocity = new Vector2(speed * mov_ x, body.velocity.y);

        isFeetTouching = Physics2D.OverlapCircle(FeetCheck.position, 0.5f, groundMask);

        //WALL JUMPING
        if (mov_x > 0)
        {
            WallCheckHit = Physics2D.Raycast(transform.position - new Vector3(0f, 0.5f, 0f), new Vector2(WallDistance, 0f), WallDistance, wallMask);
            Debug.DrawRay(transform.position - new Vector3(0f, 0.5f, 0f), new Vector2(WallDistance, 0f), Color.blue);
        }
        else
        {
            WallCheckHit = Physics2D.Raycast(transform.position - new Vector3(0f, 0.5f, 0f), new Vector2(-WallDistance, 0), WallDistance, wallMask);
            Debug.DrawRay(transform.position - new Vector3(0f, 0.5f, 0f), new Vector2(-WallDistance, 0f), Color.blue);
        }

        if (WallCheckHit && !isFeetTouching && mov_x != 0)
        {
            player_animator.SetBool("isJumping", true);
            isWallSliding = true;
            JumpTime = Time.time + WallJumpTime;
        }
        else if (JumpTime < Time.time)
        {
            isWallSliding = false;
        }

        if (isWallSliding)
            body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -WallSlideSpeed, float.MaxValue));

        //Debug.Log("Jump Countr: " + jump_count);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //jump once
            if (jump_count < max_jumps)
            {
                jump_count++;
                Jump();
            }

            clickedS = false;

            /*
            if(isGrounded){
                can2jump = true;
                body.velocity = new Vector2(body.velocity.x, 6.0f);
            }
            //jump one more time
            if(!isGrounded && can2jump){
                can2jump = false;
                body.velocity = new Vector2(body.velocity.x, 6.0f);
            }
            */
        }

        if (Input.GetKeyDown(KeyCode.S) && !clickedS)
        {

            FindObjectOfType<SoundManager>().PlaySFX("FastFall");

            clickedS = true;

            body.AddForce(Vector2.down * (quickFallRate), ForceMode2D.Impulse);

            /*
            if (body.velocity.y < 0.0f)
                body.velocity = new Vector2(body.velocity.x, 2 * body.velocity.y);
            else
                body.velocity = new Vector2(body.velocity.x, -2 * body.velocity.y);
            */

        }

        if (Time.time > player_controller.next_boost_timer && player_controller.get_dashes_value() > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && mov_x != 0)
            {
                player_controller.next_boost_timer = Time.time + player_controller.boost_cooldown;
                //player_controller.boost();

                StartCoroutine(Dash(mov_x));

            }
        }

        flip();

    }

    IEnumerator Dash(float direction)
    {

        SoundManager.instance.PlaySFX("Dash");

        player_controller.ChangeDash(-1);

        isDashing = true;
        body.velocity = new Vector2(body.velocity.x, 0f);
        body.AddForce(new Vector2(DashDistance * direction, 0f), ForceMode2D.Impulse);
        float gravity = body.gravityScale;
        body.gravityScale = 0;
        yield return new WaitForSeconds(DashSpeed);
        body.gravityScale = gravity;
        isDashing = false;
    }

    private void Jump()
    {

        SoundManager.instance.PlaySFX("Jump");

        player_animator.SetBool("isJumping", true);

        if (isWallSliding)
            body.velocity = new Vector2(body.velocity.x, jump_force*1.25f);
        else
            body.velocity = new Vector2(body.velocity.x, jump_force);
    }

    public void flip()
    {

        Vector3 characterScale = transform.localScale;

        if (Input.GetAxis("Horizontal") < 0)
        {
            characterScale.x = -1.0f;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            characterScale.x = 1.0f;
        }
        transform.localScale = characterScale;

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.collider.tag == "Ground" || collision.collider.tag == "Platform" || collision.collider.tag == "Moving_Platform") && isFeetTouching)
        {
            //Debug.Log("JKLJKL");
            //Debug.Log("GROUNDED");
            player_animator.SetBool("isJumping", false);
            //isGrounded = true;
            clickedS = false;
            jump_count = 0;
        }

        if (collision.collider.tag == "Wall" && isWallSliding)
        {
            jump_count = 0;
            //Debug.Log("CHECK");
        }

        if(collision.collider.tag == "Moving_Platform")
            transform.SetParent(collision.transform);

    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.collider.tag == "Ground" || collision.collider.tag == "Platform" || collision.collider.tag == "Moving_Platform") && isFeetTouching)
        {
            //Debug.Log("NOT GROUNDED");
            player_animator.SetBool("isJumping", true);
            //isGrounded = false;
        }

        if(collision.collider.tag == "Moving_Platform")
            transform.SetParent(null);
    }

    private void OnDrawGizmosSelected()
    {

        //GIZMOS SETTINGS
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(FeetCheck.position, 0.05f);

    }

}
