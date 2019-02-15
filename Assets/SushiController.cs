using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiController : MonoBehaviour
{
    private bool dragging;
    private float dist;
    private Color default_color;


    // Start is called before the first frame update
    void Start()
    {
        dragging = false;
        default_color = GetComponent<Renderer>().material.color;  
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !dragging)
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;
            if (Physics.Raycast(inputRay, out info))
            {
                if (info.collider.gameObject == this.gameObject)
                {
                    dragging = true;
                    GetComponent<Rigidbody>().useGravity = false;
                    GetComponent<Rigidbody>().freezeRotation = true;
                    GetComponent<Renderer>().material.color = Color.green;

                    dist = Vector3.Distance(transform.position, Camera.main.transform.position);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().freezeRotation = false;
            GetComponent<Renderer>().material.color = default_color;
        }



        if (dragging)
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 pos = camRay.GetPoint(dist);
            this.transform.position = new Vector3(pos.x, 1.5f, pos.z);
        }
    }
}
