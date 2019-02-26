using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaitenController : MonoBehaviour
{
    public int beltSpeed = 1;
    public int prevSpeed = 1;
    public Material belt_mat;
    

    // Start is called before the first frame update
    void Start()
    {
        beltSpeed = 1;
        prevSpeed = 1;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
