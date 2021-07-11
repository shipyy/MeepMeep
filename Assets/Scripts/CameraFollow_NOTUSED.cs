using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [Range(1,10)] public float smoothFactor;

    public MeepController player_obj;

    private int signal = 1;


    public void FixedUpdate(){

        //if the player is at the start
        if(player_obj.transform.position.x <= 1 && Mathf.Abs(player_obj.transform.position.x) >= -12)
            DontFollow();
        else
            Follow();

    }

    public void Follow(){

        Vector3 targetPosition;
        //float notground_offset;


        if(player_obj.mov_x > 0){
            targetPosition = target.position + new Vector3(offset.x,offset.y,offset.z);
            signal = 1;
        }
        else if(player_obj.mov_x < 0){
            targetPosition = target.position + new Vector3(-offset.x,offset.y,offset.z);
            signal = -1;
        }
        //this else is used to mantain the x-offset of the last direction the player , so it doesnt look jittery the camera movement
        else
            targetPosition = target.position + new Vector3((signal)*offset.x,offset.y,offset.z);
        

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime);

        transform.position = smoothedPosition;

    }

    public void DontFollow(){

        //i want to use this because i want to give a sense of being at the limit of the level so the player doesnt see past the wall
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, new Vector3(0,0,-10), smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothedPosition;

    }


}
