using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("GameState").GetComponent<GameState>().selectedObj == this.gameObject) {
            Debug.Log("Zushi selected");
        }
    }
}
