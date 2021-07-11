using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance;
    public GameObject Particle_explosion;
    public void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void PlayExplosion(){
        Vector2 death_location = GameObject.FindGameObjectWithTag("Player").transform.position;
        GameObject.FindGameObjectWithTag("Player").SetActive(false);
        StartCoroutine(playDeathParticle(death_location));
    }

    IEnumerator playDeathParticle(Vector2 death_location){
        Instantiate(Particle_explosion, death_location, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        ClickManager.instance.Respawn();
    }
    
}
