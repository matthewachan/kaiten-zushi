using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeltControl : MonoBehaviour
{
    GameState game_state;
    KaitenController ctrl;
    private Material mat;

    // Start is called before the first frame update
    void Start()
    {
        ctrl = GameObject.Find("Kaiten Zushi").GetComponent<KaitenController>();
        game_state = GameObject.Find("GameState").GetComponent<GameState>();
        mat = ctrl.belt_mat;
    }

 
    // Update is called once per frame
    void Update()
    {
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
                    game_state.canvas.SetActive(true);
                    GameObject.Find("Panel").transform.position = Input.mousePosition;
                    mat.color = Color.green;

                    ctrl.prevSpeed = ctrl.beltSpeed;
                    ctrl.beltSpeed = 0;

                }
            }
        }

    }


    private void OnCollisionStay(Collision collision)
    {
        GameObject other = collision.gameObject;
        Vector3 target = this.transform.TransformPoint(Vector3.left * 2.5f);
        int speed = ctrl.beltSpeed;
        if (other.tag == "Plate") {
            other.transform.position = Vector3.MoveTowards(other.transform.position, target, speed * Time.deltaTime);
        }
    }
}
