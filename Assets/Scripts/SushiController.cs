using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiController : MonoBehaviour
{
    public bool dragging;
    private float dist;
    private Color default_color;
    public bool isSpecial;
    public bool isDessert;

    GameObject inner_sauce;
    GameObject inner_marker;
    GameObject outer_marker;
    GameObject outer_sauce;

    GameObject dessertTracker;
    GameObject iceCream;
    GameObject dessertCenter;

    public GameObject breakPrefab;

    public float inner_speed;
    public float outer_speed;
    public int inner_direction;
    public int outer_direction;

    private float spawnTime;
    private float lifetime = 20f;
    //bool destroyed = false;

    private float fadeSpeed = 0.2f;


    public GameObject spherePrefab;
    GameObject sphere;
    private float sphereSize = 5f;

    GameState state;

    public GameObject saucePrefab;


    // Start is called before the first frame update
    void Start()
    {

        state = GameObject.Find("GameState").GetComponent<GameState>();
        spawnTime = Time.time;
        dragging = false;
        default_color = GetComponentsInChildren<Renderer>()[0].material.color;  

        if (isDessert)
        {
            dessertTracker = this.transform.Find("Tracker").gameObject;
            iceCream = this.transform.Find("Ice Cream").gameObject;
            dessertCenter = this.transform.Find("Center").gameObject;

            float rad = 0.1f;

            dessertCenter.transform.position = this.transform.position + new Vector3(0, 0.25f, 0);
            dessertTracker.transform.position = dessertCenter.transform.position - new Vector3(0, rad, 0);
        }

        if (isSpecial)
        {
            float r1 = 0.3f;
            float r2 = 0.6f;
            inner_speed = 20;
            outer_speed = 50;
            inner_direction = 1;
            outer_direction = 1;

            inner_marker = new GameObject();
            inner_marker.transform.SetParent(transform);
            inner_marker.transform.localScale = Vector3.zero;
            inner_marker.transform.position = transform.position;
            inner_marker.transform.position += new Vector3(r1, 0.5f, 0);


            outer_marker = new GameObject();
            outer_marker.transform.SetParent(transform);
            outer_marker.transform.localScale = Vector3.zero;
            outer_marker.transform.position = transform.position;
            outer_marker.transform.position += new Vector3(r2, 0.5f, 0);


            inner_sauce = Instantiate(saucePrefab);
            inner_sauce.name = "Inner Sauce Plate";
            inner_sauce.GetComponent<BoxCollider>().isTrigger = true;
            inner_sauce.transform.SetParent(transform);
            inner_sauce.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            inner_sauce.GetComponent<Renderer>().material.color = Color.blue;

            outer_sauce = Instantiate(saucePrefab);
            outer_sauce.name = "Outer Sauce Plate";
            outer_sauce.GetComponent<BoxCollider>().isTrigger = true;
            outer_sauce.transform.SetParent(transform);
            outer_sauce.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            outer_sauce.GetComponent<Renderer>().material.color = Color.red;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Destroy Point" && !dragging)
        {
            Break();
            //if ((Time.time - spawnTime) > 0.5f && !destroyed)
            //{
            //    destroyed = true;
            //    Break();
            //}
        }

    }

    void ChangeColor(Color c)
    {
        foreach (Renderer rend in this.GetComponentsInChildren<Renderer>())
        {
            // Don't recolor sauce
            if (rend.gameObject.tag == "Sauce")
                continue;

            float alpha = rend.material.color.a;
            rend.material.color = new Color(c.r, c.g, c.b, alpha);
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
            state.platesConsumed++;
            Destroy(this.gameObject);
        }
    }

    public void Break()
    {   
        if (state.gameObjects.Contains(this.gameObject))
        {
            state.gameObjects.Remove(this.gameObject);
            state.platesBroken++;

            GameObject smoke = Instantiate(breakPrefab);
            smoke.transform.position = this.transform.position;

            GetComponent<AudioSource>().Play();
            Destroy(this.gameObject, GetComponent<AudioSource>().clip.length);
        }

    }


    // Update is called once per frame
    void Update()
    {

        // Stale plate
        if (Time.time > spawnTime + lifetime)
            Break();

        if (isDessert)
        {
            float tiltSpeed = 2f;
            float orbitSpeed = 50f;
            dessertCenter.transform.Rotate(new Vector3(Time.deltaTime * tiltSpeed, 0, 0));
            dessertTracker.transform.RotateAround(dessertCenter.transform.position, dessertCenter.transform.forward, Time.deltaTime * orbitSpeed);
            iceCream.transform.position = dessertTracker.transform.position;
        }

        // Change plate color when selected
        if (state.selectedObj == this.gameObject)
            ChangeColor(state.selectedColor);
        else
            ChangeColor(default_color);

        // Bugfix: Wake up Rigidbody if the game is not paused
        if (GetComponent<Rigidbody>().IsSleeping() && state.paused == false)
            GetComponent<Rigidbody>().WakeUp();

        // Freeze sauce plates when a special plate is selected
        if (isSpecial && state.selectedObj != this.gameObject)
        {

            inner_marker.transform.RotateAround(transform.position, transform.up, Time.deltaTime * inner_speed * inner_direction);
            inner_sauce.transform.position = inner_marker.transform.position;

            outer_marker.transform.RotateAround(transform.position, transform.up, Time.deltaTime * outer_speed * outer_direction);
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
            GetComponent<Rigidbody>().freezeRotation = true;
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

            if (diff < radius)
                this.transform.position = new Vector3(pos.x, 1.5f, pos.z);
        }
    }
}
