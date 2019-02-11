using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltControl : MonoBehaviour
{
    public float speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionStay(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.name == "Sushi Plate") {
            other.GetComponent<Rigidbody>().AddForce(-speed, 0, 0);
        }
    }
}
