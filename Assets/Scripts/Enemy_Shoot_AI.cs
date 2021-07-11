using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shoot_AI : MonoBehaviour
{

    public Transform defaultPosition;
    public Transform PointA;
    public Transform PointB;
    public float speed;
    private Transform player;
    public float lineofsight;
    public LayerMask layerMask;
    public RaycastHit2D isPlayerDetected;
    public Transform playerFeet;
    private Vector3 dirToPlayerHead;
    private Vector3 dirToPlayerFeet;
    private Vector3 dirToPlayerCenter;

    private bool isHitingPlayer;
    private bool isHitingHead;
    private bool isHitingFeet;

    public GameObject bullet;

    public float fireRate;
    public float nextFireTime;

    public bool UseBoundaries;
    public bool Move;
    public float bulletSpeed;

    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player").transform;
        nextFireTime = Time.time;

        playerFeet = GameObject.FindGameObjectWithTag("Player_Feet").transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (player != null)
        {

            this.dirToPlayerHead = (player.transform.position - this.transform.position).normalized;
            this.dirToPlayerFeet = (playerFeet.position - this.transform.position).normalized;

            //WHEN V3RD VALUE IN .LERP IS 0.5F IT RETURNS THE MIDPOINT BETWEEN 2POINTS
            this.dirToPlayerCenter = Vector3.Lerp(player.position, playerFeet.position, 0.5f);


            RaycastHit2D isPlayerHeadDetected = Physics2D.Raycast(this.transform.position, dirToPlayerHead, lineofsight, layerMask);
            Debug.DrawRay(this.transform.position, dirToPlayerHead * lineofsight, Color.blue);
            RaycastHit2D isPlayerFeetDetected = Physics2D.Raycast(this.transform.position, dirToPlayerFeet, lineofsight, layerMask);
            Debug.DrawRay(this.transform.position, dirToPlayerFeet * lineofsight, Color.red);

            //Debug.DrawRay(playerFeet.position,  transform.position - playerFeet.position, Color.red);

            if (isPlayerHeadDetected.collider != null)
                if (isPlayerHeadDetected.collider.tag == "Player")
                    this.isHitingHead = true;
                else
                    this.isHitingHead = false;

            if (isPlayerFeetDetected.collider != null)
                if (isPlayerFeetDetected.collider.tag == "Player")
                    this.isHitingFeet = true;
                else
                    this.isHitingFeet = false;

            if (isPlayerHeadDetected.collider == null)
                this.isHitingHead = false;

            if (isPlayerFeetDetected.collider == null)
                this.isHitingFeet = false;


            this.isHitingPlayer = this.isHitingHead || this.isHitingFeet;

            if (this.isHitingPlayer)
            {

                //SHOOT
                if (nextFireTime < Time.time)
                {
                    Instantiate(bullet, transform.position, Quaternion.identity, transform);
                    nextFireTime = Time.time + fireRate;
                }

                if (Move)
                {

                    if (UseBoundaries)
                    {
                        if (this.transform.position.x >= PointB.position.x || this.transform.position.x <= PointA.position.x)
                            this.transform.position = this.transform.position;
                        else
                            this.transform.position = Vector2.MoveTowards(this.transform.position, new Vector2(player.position.x, this.transform.position.y), speed * Time.deltaTime);
                    }
                    else
                    {
                        //MOVE ENEMY
                        //transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
                        //if i wanna do like make it like a gun that just slide across the ceiling use this instead
                        transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
                    }

                }

            }
            else if (!isHitingPlayer)
                transform.position = Vector2.MoveTowards(this.transform.position, this.defaultPosition.position, 2 * speed * Time.deltaTime);


            //ORIENTATION
            if (this.transform.position.x > player.transform.position.x)
                this.GetComponent<SpriteRenderer>().flipX = false;
            else
                this.GetComponent<SpriteRenderer>().flipX = true;

        }

    }

    public bool isHead()
    {
        return isHitingHead;
    }

    public bool isFeet()
    {
        return isHitingFeet;
    }

    public Vector3 getPlayerHead_Direction()
    {
        return dirToPlayerHead;
    }

    public Vector3 getPlayerFeet_Direction()
    {
        return dirToPlayerFeet;
    }

    public Vector3 getPlayerCenter_Direction()
    {
        return dirToPlayerCenter;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lineofsight);
    }

    public float getBulletSpeed(){
        return bulletSpeed;
    }

}
