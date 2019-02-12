using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltControl : MonoBehaviour
{
    public float speed = 10;
    public GameObject menu;
    private GameObject ui;


    private bool selected;

    // Start is called before the first frame update
    void Start()
    {
       
        selected = false;
    }

    void CloseWindow()
    {
        Destroy(ui);
        selected = false;
    }
    // Update is called once per frame
    void Update()
    {
        GameState game_state = GameObject.Find("GameState").GetComponent<GameState>();



        if (selected) {
            GameObject.Find("Exit").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(CloseWindow);
            
        }


        if (!selected && game_state.selectedObj == this.gameObject) {
            selected = true;
            
            GameObject canvas = GameObject.Find("Canvas");
            canvas.SetActive(true);
            ui = Instantiate(menu, canvas.transform);
            ui.GetComponent<RectTransform>().anchoredPosition = this.transform.position;
        }


        if (game_state.selectedObj != this.gameObject) {
            selected = false;
        }


        //if (!selected) {
        //    Destroy(ui);
        //}
    }


    private void OnCollisionStay(Collision collision)
    {
        GameObject other = collision.gameObject;
        Vector3 dir = this.transform.TransformDirection(-1, 0, 0);
        dir *= speed;
        if (other.name == "Sushi Plate") {
            other.GetComponent<Rigidbody>().AddRelativeForce(dir);
        }
    }
}
