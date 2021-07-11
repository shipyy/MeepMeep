using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Scroller : MonoBehaviour
{
    // Start is called before the first frame update

    public float scroll_speed;
    private Vector2 startPosistion;

    void Start()
    {
        startPosistion = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * scroll_speed * Time.deltaTime);
        if(transform.position.x < -22.54f){
            transform.position = startPosistion;
        }
    }
}
