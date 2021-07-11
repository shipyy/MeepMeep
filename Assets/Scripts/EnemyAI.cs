using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAI : MonoBehaviour
{
    public float mov_Speed;
    public bool mustPatrol;
    public Rigidbody2D rb;
    private bool mustTurn;
    public Transform groundCheckpos;
    public Transform wallCheckpos;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public Collider2D bodyCollider;
    private bool wallCollided;
    public float jumpRange;
    public float jumpForce;
    public Transform player;
    public bool doJump;

    private Vector3 dirToPlayer;

    public float jumpCooldown;
    private float nextJumptime;

    public bool isJumping;

    public bool isGrounded;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        mustPatrol = true;
        nextJumptime = Time.time;
    }

    void Update()
    {   
        isGrounded = !mustTurn;

        if (nextJumptime < Time.time)
        {
            if (Vector2.Distance(this.transform.position, player.position) < jumpRange && doJump)
            {   
                //ONLY LEAP IF THE RAT IS FACING THE PLAYER
                if ((player.position.x > transform.position.x && rb.velocity.x > 0) || player.position.x < transform.position.x && rb.velocity.x < 0)
                {
                    Debug.Log("IN RANGE");
                    StartCoroutine(Jump());
                    nextJumptime = Time.time + jumpCooldown;
                }
            }

        }

    }

    IEnumerator Jump()
    {
        mustPatrol = false;
        isJumping = true;

        dirToPlayer = (player.transform.position - transform.position).normalized;

        rb.AddForce(dirToPlayer * jumpForce);

        yield return new WaitForSeconds(0.5f);
        isJumping = false;

        mustPatrol = true;
    }


    void Patrol()
    {
        //I USE THE  (RB.VELOCITY.Y == 0F) -> BECAUSE I ONLY WANT THE ENEMY TO FLIP() ON IT IS ON THE GROUND AND NOT WHEN IT IS JUMPING
        //THIS CONDITION BASICALLY ACTS LIKE A GROUND CHECK
        if ((mustTurn || wallCollided) && !isJumping && isGrounded)
            Flip();

        rb.velocity = new Vector2(mov_Speed * Time.fixedDeltaTime, rb.velocity.y);
    }

    void FixedUpdate()
    {

        if (mustPatrol)
        {
            
            mustTurn = !Physics2D.OverlapCircle(groundCheckpos.position, 0.1f, groundLayer);
            // i only want to have mustTurn true when the return of the funtion is false
            // for instance, i must turn direction when the collider is not colliding with the ground (which means i reached the end of the platform)

            wallCollided = Physics2D.OverlapCircle(wallCheckpos.position, 0.1f, wallLayer);
            //if i am colliding with the wall i want to have wallCollided with true value

            Patrol();
        }

    }

    void Flip()
    {
        //since it hit something it doesnt dont need to patrol 
        mustPatrol = false;

        //next i actually flip the enemy
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        mov_Speed *= -1;

        //since it is now flipped it must patrol again
        mustPatrol = true;
    }

    private void OnDrawGizmosSelected()
    {

        //GIZMOS SETTINGS
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(groundCheckpos.position, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(wallCheckpos.position, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, jumpRange);

    }

}
