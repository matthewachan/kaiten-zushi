using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public GameObject selectedObj;
    public GameObject canvas;
    //public bool paused;


    public ArrayList gameObjects;
    public string difficulty;

    public bool paused;

    // Start is called before the first frame update
    void Start()
    {
        gameObjects = new ArrayList();

        canvas = GameObject.Find("Canvas");
        GameObject.Find("Plus").GetComponent<Button>().onClick.AddListener(IncreaseSpeed);
        GameObject.Find("Minus").GetComponent<Button>().onClick.AddListener(ReduceSpeed);
        GameObject.Find("Escape").GetComponent<Button>().onClick.AddListener(CloseWindow);
        canvas.SetActive(false);

        //paused = false;
        difficulty = "easy";

    }

    void CloseWindow()
    {
        KaitenController ctrl = GameObject.Find("Kaiten Zushi").GetComponent<KaitenController>();

        canvas.SetActive(false);
        ctrl.belt_mat.color = Color.gray;
        ctrl.beltSpeed = ctrl.prevSpeed;
        GameObject.Find("GameState").GetComponent<GameState>().paused = false;
        WakeUpPlates();
    }

    void ReduceSpeed()
    {
        KaitenController ctrl = GameObject.Find("Kaiten Zushi").GetComponent<KaitenController>();
        if (ctrl.prevSpeed > 0)
            ctrl.prevSpeed--;

    }

    void WakeUpPlates()
    {
        foreach (GameObject plate in GameObject.FindGameObjectsWithTag("Plate"))
        {
            plate.GetComponent<Rigidbody>().WakeUp();
        }
    }

    void IncreaseSpeed()
    {
        GameObject.Find("Kaiten Zushi").GetComponent<KaitenController>().prevSpeed++;

    }

    // Update is called once per frame
    void Update()
    {
        if (canvas.active)
            GameObject.Find("Value").GetComponent<Text>().text = GameObject.Find("Kaiten Zushi").GetComponent<KaitenController>().prevSpeed.ToString();

    }
}
