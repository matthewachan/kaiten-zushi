using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefController : MonoBehaviour
{
    public GameObject sushiPlate;
    public GameObject specialPlate;
    public GameObject dessertPlate;

    private int coolDown = 5;
    private float timeStamp;


    // Start is called before the first frame update
    void Start()
    {
        timeStamp = Time.time;
    }


    void SpawnPlate()
    {
        Debug.Log("Instantiating Plate");
        int rand = Random.Range(0, 3);
        GameObject plate;
        if (rand == 0)
            plate = Instantiate(sushiPlate);
        else if (rand == 1)
            plate = Instantiate(sushiPlate);
        else
            plate = Instantiate(sushiPlate);
        plate.transform.position = GameObject.Find("Spawn Point").transform.position;
        GameState state = GameObject.Find("GameState").GetComponent<GameState>();
        state.gameObjects.Enqueue(plate);
        Debug.Log("Number of plates " + state.gameObjects.Count);
    }


    // Update is called once per frame
    void Update()
    {
        if (timeStamp <= Time.time)
        {
            SpawnPlate();
            timeStamp = Time.time + coolDown;
        }

        // Focus spotlight on the first plate
        GameState state = GameObject.Find("GameState").GetComponent<GameState>();
        if (state.gameObjects.Count > 0)
        {
            GameObject light = GameObject.Find("Spot Light");
            GameObject plate = state.gameObjects.Peek();
            light.transform.LookAt(plate.transform);
        }
    }
}
