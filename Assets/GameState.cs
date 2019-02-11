using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0)) {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] info = Physics.RaycastAll(inputRay);
            Debug.Log(info[1].collider.gameObject.name);
            //foreach (RaycastHit hit in info) {
            //    Debug.Log(hit.collider.gameObject.name);
            //}
        }
        

    }
}
