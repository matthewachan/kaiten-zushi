using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public GameObject selectedObj;
    public Color selectedColor = new Color(0, 1, 0);

    public GameObject canvas;
    //public bool paused;


    public ArrayList gameObjects;
    public string difficulty;

    public bool paused;

    public int platesBroken;
    public int platesConsumed;

    public GameObject beltPanel;
    public GameObject saucePanel;
    

    // Start is called before the first frame update
    void Start()
    {

        gameObjects = new ArrayList();

        canvas = GameObject.Find("Canvas");

        beltPanel = GameObject.Find("Belt Panel");
        beltPanel.transform.Find("Plus").GetComponent<Button>().onClick.AddListener(IncreaseSpeed);
        beltPanel.transform.Find("Minus").GetComponent<Button>().onClick.AddListener(ReduceSpeed);
        beltPanel.transform.Find("Escape").GetComponent<Button>().onClick.AddListener(CloseWindow);
        beltPanel.SetActive(false);
    
        saucePanel = GameObject.Find("Sauce Panel");
        saucePanel.transform.Find("Slider").GetComponent<Slider>().onValueChanged.AddListener(delegate { SetOrbitSpeed(); });
        saucePanel.transform.Find("Flip Button").GetComponent<Button>().onClick.AddListener(ChangeOrbitDirection);
        //beltPanel.transform.Find("Plus").GetComponent<Button>().onClick.AddListener(IncreaseSpeed);
        //beltPanel.transform.Find("Minus").GetComponent<Button>().onClick.AddListener(ReduceSpeed);
        saucePanel.transform.Find("Escape").GetComponent<Button>().onClick.AddListener(CloseWindow);
        saucePanel.SetActive(false);

        paused = false;
        difficulty = "easy";
        platesBroken = 0;
        platesConsumed = 0;

    }

    void ChangeOrbitDirection()
    {
        if (selectedObj.name == "Inner Sauce Plate")
            selectedObj.GetComponentInParent<SushiController>().inner_direction *= -1;
        else
            selectedObj.GetComponentInParent<SushiController>().outer_direction *= -1;
        
    }
    void SetOrbitSpeed()
    {
        float newSpeed = GameObject.Find("Slider").GetComponent<Slider>().value;
        SushiController ctrl = selectedObj.GetComponentInParent<SushiController>();
        if (selectedObj.name == "Inner Sauce Plate")
            ctrl.inner_speed =  newSpeed;

        else
            ctrl.outer_speed = newSpeed;
    }

    void CloseWindow()
    {
        KaitenController ctrl = GameObject.Find("Kaiten Zushi").GetComponent<KaitenController>();

        if (beltPanel.activeInHierarchy)
        {
            beltPanel.SetActive(false);
            ctrl.belt_mat.color = Color.gray;
            ctrl.beltSpeed = ctrl.prevSpeed;
            GameObject.Find("GameState").GetComponent<GameState>().paused = false;
            WakeUpPlates();
        }
        else if (saucePanel.activeInHierarchy)
        {
            saucePanel.SetActive(false);
        }
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
            if (plate.GetComponent<Rigidbody>() != null)
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

        GameObject[] systems = GameObject.FindGameObjectsWithTag("ParticleSystem");

        // Update broken and consumed plate count
        GameObject.Find("nConsumed").GetComponent<Text>().text = platesConsumed.ToString();
        GameObject.Find("nBroken").GetComponent<Text>().text = platesBroken.ToString();


        if (Input.GetMouseButtonDown(0))
        {
            /* Return if the mouse is over a UI object */
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                return;

            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;
            if (Physics.Raycast(inputRay, out info))
            {
                selectedObj = info.collider.gameObject;
                if (selectedObj.tag == "Sauce")
                {
                    Debug.Log("Selected sauce");
                    saucePanel.SetActive(true);
                    SushiController sushiController = selectedObj.GetComponentInParent<SushiController>();
                    if (selectedObj.name == "Inner Sauce Plate")
                        saucePanel.transform.Find("Slider").GetComponent<Slider>().value = sushiController.inner_speed;
                    else
                        saucePanel.transform.Find("Slider").GetComponent<Slider>().value = sushiController.outer_speed;

                }
            }
        }


        foreach (GameObject ps in systems)
        {
            if (!ps.GetComponent<ParticleSystem>().IsAlive())
                Destroy(ps);
        }
        if (beltPanel.activeInHierarchy)
            GameObject.Find("Value").GetComponent<Text>().text = GameObject.Find("Kaiten Zushi").GetComponent<KaitenController>().prevSpeed.ToString();

    }
}
