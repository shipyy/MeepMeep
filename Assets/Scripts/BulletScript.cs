using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject player;
    public float bulletSpeed;
    private Vector2 target;

    public Enemy_Shoot_AI enemy;

    private Rigidbody2D bulletRB;

    void Start()
    {
        
        this.transform.localScale = new Vector3(-0.25f, -0.25f, -0.25f);

        SoundManager.instance.PlaySFX("Bullet");

        bulletRB = GetComponent<Rigidbody2D>();

        player = GameObject.FindGameObjectWithTag("Player");

        //Enemy_Shoot_AI enemy = FindObjectOfType<Enemy_Shoot_AI>();
        enemy = transform.parent.GetComponent<Enemy_Shoot_AI>();
        bulletSpeed = enemy.getBulletSpeed();
        
        //I NEED TO SET PARENT AS NULL
        //BECAUSE IF THE PARENT OF THIS BULLET MOVES POSITION INT HE WORLD SO DOES THE BULLET
        //WHICH WOULD CAUSE IT TO NOT MOVE IN A STRAIGHT LINE
        this.transform.SetParent(null);

        //normalized vector makes the bullet take only the direction in consideration, disregarding the distance on both axis
        if(enemy.isHead() && !enemy.isFeet()){
            Debug.Log("HEAD");
            target = enemy.getPlayerHead_Direction() * bulletSpeed;
        }
        else if(!enemy.isHead() && enemy.isFeet()){
            Debug.Log("FEET");
            target = enemy.getPlayerFeet_Direction() * bulletSpeed;
        }
        else if(enemy.isHead() && enemy.isFeet()){
            Debug.Log("CENTER");
            //WHEN V3RD VALUE IN .LERP IS 0.5F IT RETURNS THE MIDPOINT BETWEEN 2POINTS
            //Vector3 player_center = Vector3.Lerp(GameObject.FindGameObjectWithTag("Player").transform.position, enemy.playerFeet.position, 0.5f);
            target = (enemy.getPlayerCenter_Direction() - transform.position).normalized * bulletSpeed;
        }
        
        Debug.Log("-----------");

        Debug.Log(target);

        bulletRB.velocity = target;

    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(target);
        Destroy(gameObject);
    }

    /*
    void Update(){

        transform.position = Vector2.MoveTowards(transform.position, target, bulletSpeed * Time.deltaTime);
    
        if(transform.position.x == target.x && transform.position.y == target.y)
            Destroy(gameObject);

    }
    */

}
