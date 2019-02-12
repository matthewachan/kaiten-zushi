using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public GameObject selectedObj;
    public bool paused;

    public string difficulty;

    // Start is called before the first frame update
    void Start()
    {
        paused = false;
        difficulty = "easy";
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0)) {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;
            if (Physics.Raycast(inputRay, out info)) {
                selectedObj = info.collider.gameObject;
            }

        }
        
    }
}
