using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Associator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager GM;

    void Start()
    {
        GM = FindObjectOfType<GameManager>();
    }

}
