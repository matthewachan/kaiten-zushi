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

    private void OnCollisionStay(Collision collision)
    {
        GameObject other = collision.gameObject;
        Vector3 target = this.transform.TransformPoint(Vector3.back + (Vector3.left / 3));
        float speed = GameObject.Find("Kaiten Zushi").GetComponent<KaitenController>().beltSpeed;
        if (other.name == "Sushi Plate")
        {
            other.transform.position = Vector3.MoveTowards(other.transform.position, target, speed * Time.deltaTime);
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.name == "Sushi Plate") {
    //        other.transform.position = this.transform.position + new Vector3(0, 1, 0);
    //        other.transform.rotation = Quaternion.identity;
    //        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    Vector3 dir = this.transform.TransformDirection(0, 0, -1);
    //    dir *= 0.5f;
    //    if (other.name == "Sushi Plate") {
    //        Vector3 pos = other.transform.position;
    //        other.transform.rotation = Quaternion.identity;
    //        other.transform.position += dir;
    //    }
    //}
}
