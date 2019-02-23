﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiController : MonoBehaviour
{
    public bool dragging;
    private float dist;
    private Color default_color;
    public bool isSpecial;

    GameObject inner_sauce;
    GameObject inner_marker;
    GameObject outer_marker;
    GameObject outer_sauce;

    public GameObject breakPrefab;

    float s1;
    float s2;

    private float spawnTime;
    bool destroyed = false;

    private float fadeSpeed = 0.2f;


    public GameObject spherePrefab;
    GameObject sphere;
    private float sphereSize = 5f;

    GameState state;

    private AudioSource audioSrc;
    public AudioClip audioClip;


    // Start is called before the first frame update
    void Start()
    {
        //audioSrc = new AudioSource();
        //audioSrc.clip = audioClip;
        //audioSrc.Play();

        state = GameObject.Find("GameState").GetComponent<GameState>();
        spawnTime = Time.time;
        //Debug.Log("Spawning in at " + spawnTime);
        dragging = false;
        default_color = GetComponentsInChildren<Renderer>()[0].material.color;  

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
        if (other.name == "Spawn Point" && !dragging)
        {
            if ((Time.time - spawnTime) > 0.5f && !destroyed)
            {
                state.gameObjects.Remove(state.gameObjects[0]);
                destroyed = true;
                Break();
            }
        }



    }

    private void OnCollisionStay(Collision collision)
    {
        GameObject other = collision.gameObject;
   
        if (other.name == "Table")
        {
            if (MakeTransparent() < 0.1f)
            {
                Consume();
            }

        }
        else if (other.name == "Floor")
        {
            Break();
        }
        else if (other.tag == "Plate")
        {
            Break();
        }

    }

    private float MakeTransparent()
    {
        Color old;


        float alpha = 1;
        foreach (Renderer child in this.GetComponentsInChildren<Renderer>())
        {
            old = child.material.color;
            alpha = old.a - Time.deltaTime * fadeSpeed;
            child.material.color = new Color(old.r, old.g, old.b, alpha);
        }
        return alpha;
    }

    public void Consume()
    {
        if (state.gameObjects.Contains(this.gameObject))
        {
            state.gameObjects.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void Break()
    {   
        if (state.gameObjects.Contains(this.gameObject))
        {
            state.gameObjects.Remove(this.gameObject);
            GameObject smoke = Instantiate(breakPrefab);
            smoke.transform.position = this.transform.position;

            GetComponent<AudioSource>().Play();
            Destroy(this.gameObject, 1);
        }

    }


    // Update is called once per frame
    void Update()
    {
        //GetComponent<AudioSource>().Play();

        if (GetComponent<Rigidbody>().IsSleeping() && state.paused == false)
        {
            GetComponent<Rigidbody>().WakeUp();
        }
        if (isSpecial && !dragging)
        {
            inner_marker.transform.RotateAround(transform.position, transform.up, Time.deltaTime * s1);
            inner_sauce.transform.position = inner_marker.transform.position;

            outer_marker.transform.RotateAround(transform.position, transform.up, Time.deltaTime * s2);
            outer_sauce.transform.position = outer_marker.transform.position;
        }

        // Instant where user begins dragging
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
                    GetComponentsInChildren<Renderer>()[0].material.color = Color.green;

                    dist = Vector3.Distance(transform.position, Camera.main.transform.position);
                    sphere = Instantiate(spherePrefab);
                    sphere.transform.localScale = new Vector3(sphereSize, sphereSize, sphereSize);
                    sphere.transform.position = this.transform.position;

                    
                }
            }
        }
        // User releases plate
        else if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().freezeRotation = false;
            GetComponentsInChildren<Renderer>()[0].material.color = default_color;

            Destroy(sphere);
        }
        // Continued dragging
        if (dragging)
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 pos = camRay.GetPoint(dist);

            float radius = sphere.GetComponent<Renderer>().bounds.extents.magnitude;
            //radius = sphereSize;
            float diff = Vector3.Distance(pos, sphere.transform.position);
            radius /= 2;
            Debug.Log(radius);

            if (diff < radius)
                this.transform.position = new Vector3(pos.x, 1.5f, pos.z);
        }
    }
}
