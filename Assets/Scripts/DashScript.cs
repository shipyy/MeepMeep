using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashScript : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        MeepController controller = other.GetComponent<MeepController>();

        if (controller != null)
        {   
            if(controller.get_dashes_value() < controller.get_maxdashes_value()){
                SoundManager.instance.PlaySFX("Collect");
                controller.ChangeDash(1);
                Destroy(gameObject);
            }
        }
    }
}
