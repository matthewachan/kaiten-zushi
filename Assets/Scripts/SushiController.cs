﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiController : MonoBehaviour
{
    private bool dragging;
    private float dist;
    private Color default_color;
    public bool isSpecial;

    GameObject inner_sauce;
    GameObject inner_marker;
    GameObject outer_marker;
    GameObject outer_sauce;

    float s1;
    float s2;

    private float spawnTime;
    bool destroyed = false;


    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time;
        Debug.Log("Spawning in at " + spawnTime);
        dragging = false;
        default_color = GetComponent<Renderer>().material.color;  

        if (isSpecial)
        {
            float r1 = 0.3f;
            float r2 = 0.6f;
            s1 = 20;
            s2 = 50;

            inner_marker = new GameObject();
            inner_marker.transform.SetParent(transform);
            inner_marker.transform.localScale = Vector3.zero;
            inner_marker.transform.position = transform.position;
            inner_marker.transform.position += new Vector3(r1, 2, 0);


            outer_marker = new GameObject();
            outer_marker.transform.SetParent(transform);
            outer_marker.transform.localScale = Vector3.zero;
            outer_marker.transform.position = transform.position;
            outer_marker.transform.position += new Vector3(r2, 2, 0);


            inner_sauce = GameObject.CreatePrimitive(PrimitiveType.Cube);
            inner_sauce.GetComponent<BoxCollider>().enabled = false;
            inner_sauce.transform.SetParent(transform);
            inner_sauce.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            inner_sauce.GetComponent<Renderer>().material.color = Color.blue;

            outer_sauce = GameObject.CreatePrimitive(PrimitiveType.Cube);
            outer_sauce.GetComponent<BoxCollider>().enabled = false;
            outer_sauce.transform.SetParent(transform);
            outer_sauce.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            outer_sauce.GetComponent<Renderer>().material.color = Color.red;

        }
    }

    private void OnTriggerEnter(Collider other)
    { 
        if (other.name == "Spawn Point" && (Time.time - spawnTime) > 5 && !destroyed)
        {
            GameObject.Find("GameState").GetComponent<GameState>().gameObjects.Dequeue();
            destroyed = true;
            Debug.Log("Destroy plate");
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpecial)
        {
            inner_marker.transform.RotateAround(transform.position, transform.up, Time.deltaTime * s1);
            inner_sauce.transform.position = inner_marker.transform.position;

            outer_marker.transform.RotateAround(transform.position, transform.up, Time.deltaTime * s2);
            outer_sauce.transform.position = outer_marker.transform.position;
        }

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
