using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefController : MonoBehaviour
{
    public GameObject sushiPlate;
    public GameObject specialPlate;
    public GameObject dessertPlate;

    public int coolDown = 5;
    private float timeStamp;

    private bool setPause = false;
    private float freezeDelta;

    private GameState state;

    // Start is called before the first frame update
    void Start()
    {
        state = GameObject.Find("GameState").GetComponent<GameState>();
        timeStamp = Time.time;
    }


    void SpawnPlate()
    {
        //Debug.Log("Instantiating Plate");
        int rand = Random.Range(0, 3);
        GameObject plate;
        if (rand == 0)
            plate = Instantiate(sushiPlate);
        else if (rand == 1)
            plate = Instantiate(specialPlate);
        else
            plate = Instantiate(dessertPlate);
        plate.transform.position = GameObject.Find("Spawn Point").transform.position;
        GameState state = GameObject.Find("GameState").GetComponent<GameState>();
        state.gameObjects.Add(plate);
        //Debug.Log("Number of plates " + state.gameObjects.Count);
    }

    public void ResetColor()
    {
        this.GetComponent<Renderer>().material.color = Color.blue;
    }

    // Update is called once per frame
    void Update()
    {
        GameState state = GameObject.Find("GameState").GetComponent<GameState>();

        if (Input.GetMouseButtonDown(0))
        {
            /* Return if the mouse is over a UI object */
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                return;

            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;
            if (Physics.Raycast(inputRay, out info))
            {
                if (info.collider.gameObject == this.gameObject)
                {
                    this.GetComponent<Renderer>().material.color = Color.green;
                }
                else
                {
                    ResetColor();
                }

            }
        }


        // Freeze cooldown timer if the game is in a paused state
        if (state.paused)
        {
            if (!setPause)
            {
                freezeDelta = timeStamp - Time.time;
                setPause = true;
            }
            else 
                timeStamp = Time.time + freezeDelta;
        }
        else
        {
            setPause = false;
        }

        if (!state.paused && timeStamp <= Time.time)
        {
            SpawnPlate();
            timeStamp = Time.time + coolDown;
        }

        // Focus spotlight on the first plate
        if (state.gameObjects.Count > 0)
        {
            GameObject light = GameObject.Find("Spot Light");
            GameObject plate = (GameObject) state.gameObjects[0];
            light.transform.LookAt(plate.transform);
        }
    }
}
