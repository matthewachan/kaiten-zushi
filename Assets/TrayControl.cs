using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Sushi Plate") {
            other.transform.position = this.transform.position + new Vector3(0, 1, 0);
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    GameObject other = collision.gameObject;
    //    if (other.name == "Sushi Plate") {
    //        other.transform.position = this.transform.position + new Vector3(0, 1.5f, 0);
    //    }
    //}
}
