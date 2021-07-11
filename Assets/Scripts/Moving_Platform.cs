using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving_Platform : MonoBehaviour
{
    public Transform platform;
    public List<Transform> points;
    private int current_point_index;

    public float movSpeed;

    void Update(){

        platform.position = Vector2.MoveTowards(platform.position, points[current_point_index].position, movSpeed * Time.deltaTime);

        if( Vector2.Distance(platform.position, points[current_point_index].position) < 0.1f ){
            if(current_point_index == points.Count - 1)
                current_point_index = 0;
            else
                current_point_index++;
        }       

    }

}
