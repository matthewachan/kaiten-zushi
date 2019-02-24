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
    public GameObject camPanel;

    //bool restaurantMode;

    Color highlightColor = Color.green;

    Vector3 camPos;
    Quaternion camRot;

    // Start is called before the first frame update
    void Start()
    {
        //restaurantMode = true;

        gameObjects = new ArrayList();

        canvas = GameObject.Find("Canvas");

        beltPanel = GameObject.Find("Belt Panel");
        beltPanel.transform.Find("Slider").GetComponent<Slider>().onValueChanged.AddListener(SetBeltSpeed);
        //beltPanel.transform.Find("Plus").GetComponent<Button>().onClick.AddListener(IncreaseSpeed);
        //beltPanel.transform.Find("Minus").GetComponent<Button>().onClick.AddListener(ReduceSpeed);
        beltPanel.transform.Find("Escape").GetComponent<Button>().onClick.AddListener(CloseWindow);
        beltPanel.SetActive(false);
    
        saucePanel = GameObject.Find("Sauce Panel");
        saucePanel.transform.Find("Slider").GetComponent<Slider>().onValueChanged.AddListener(delegate { SetOrbitSpeed(); });
        saucePanel.transform.Find("Flip Button").GetComponent<Button>().onClick.AddListener(ChangeOrbitDirection);
        saucePanel.transform.Find("Escape").GetComponent<Button>().onClick.AddListener(CloseWindow);
        saucePanel.SetActive(false);

        camPos = Camera.main.transform.position;
        camRot = Camera.main.transform.rotation;

        GameObject.Find("Camera Button").GetComponent<Button>().onClick.AddListener(OpenCamPanel);

        camPanel = GameObject.Find("Camera Panel");
        ToggleRestaurantMode();
        SetDifficultyEasy();


        camPanel.transform.Find("Yaw").GetComponentInChildren<Slider>().onValueChanged.AddListener(SetCameraRotation);
        camPanel.transform.Find("Pitch").GetComponentInChildren<Slider>().onValueChanged.AddListener(SetCameraRotation);


        camPanel.transform.Find("Hard").GetComponent<Button>().onClick.AddListener(SetDifficultyHard);
        camPanel.transform.Find("Easy").GetComponent<Button>().onClick.AddListener(SetDifficultyEasy);
        camPanel.transform.Find("Restaurant Mode").GetComponent<Button>().onClick.AddListener(ToggleRestaurantMode);
        camPanel.transform.Find("Player Mode").GetComponent<Button>().onClick.AddListener(TogglePlayerMode);
        camPanel.transform.Find("Escape").GetComponent<Button>().onClick.AddListener(CloseWindow);
        camPanel.transform.Find("Forward").GetComponent<Button>().onClick.AddListener(MoveCameraForward);
        camPanel.transform.Find("Backward").GetComponent<Button>().onClick.AddListener(MoveCameraBackward);

        camPanel.SetActive(true);

        paused = false;
        difficulty = "easy";
        platesBroken = 0;
        platesConsumed = 0;

    }

    void SetBeltSpeed(float value)
    {
        KaitenController ctrl = GameObject.Find("Kaiten Zushi").GetComponent<KaitenController>();
        ctrl.prevSpeed = (int) value;
        //if (ctrl.prevSpeed > 0)
            //ctrl.prevSpeed--;
    }

    void SetCameraRotation(float value)
    {
        Slider pitchSlider = camPanel.transform.Find("Pitch").GetComponentInChildren<Slider>();
        float pitchAngle = Mathf.Lerp(-180, 180, pitchSlider.normalizedValue);

        Slider yawSlider = camPanel.transform.Find("Yaw").GetComponentInChildren<Slider>();
        float yawAngle = Mathf.Lerp(-180, 180, yawSlider.normalizedValue);

        Camera.main.transform.localRotation = Quaternion.Euler(pitchAngle, yawAngle, 0);
    }


    void OpenCamPanel()
    {
        camPanel.SetActive(true);
        DeactivateBeltPanel();
        saucePanel.SetActive(false);
    }

    void ToggleRestaurantMode()
    {
        ColorBlock cb = camPanel.transform.Find("Restaurant Mode").GetComponent<Button>().colors;
        cb.normalColor = highlightColor;
        camPanel.transform.Find("Restaurant Mode").GetComponent<Button>().colors = cb;

        cb = camPanel.transform.Find("Player Mode").GetComponent<Button>().colors;
        cb.normalColor = Color.white;
        camPanel.transform.Find("Player Mode").GetComponent<Button>().colors = cb;

        Camera.main.transform.position = camPos;
        Camera.main.transform.rotation = camRot;


        camPanel.transform.Find("Pitch").gameObject.SetActive(false);
        camPanel.transform.Find("Yaw").gameObject.SetActive(false);
        camPanel.transform.Find("Speed").gameObject.SetActive(false);
        camPanel.transform.Find("Backward").gameObject.SetActive(false);
        camPanel.transform.Find("Forward").gameObject.SetActive(false);
    }

    public void MoveCameraForward()
    {
        float speed = camPanel.transform.Find("Speed").GetComponentInChildren<Slider>().value;
        Camera.main.transform.Translate(Vector3.forward * speed);
    }

    void MoveCameraBackward()
    {
        float speed = camPanel.transform.Find("Speed").GetComponentInChildren<Slider>().value;
        Camera.main.transform.Translate(Vector3.back * speed);
    }

    void TogglePlayerMode()
    {
        ColorBlock cb = camPanel.transform.Find("Player Mode").GetComponent<Button>().colors;
        cb.normalColor = highlightColor;
        camPanel.transform.Find("Player Mode").GetComponent<Button>().colors = cb;

        cb = camPanel.transform.Find("Restaurant Mode").GetComponent<Button>().colors;
        cb.normalColor = Color.white;
        camPanel.transform.Find("Restaurant Mode").GetComponent<Button>().colors = cb;

        Camera.main.transform.position = camPos;
        Camera.main.transform.rotation = camRot;

        // Update slider values
        camPanel.transform.Find("Yaw").GetComponentInChildren<Slider>().value = Camera.main.transform.rotation.eulerAngles.y;
        camPanel.transform.Find("Pitch").GetComponentInChildren<Slider>().value = Camera.main.transform.rotation.eulerAngles.x;

        camPanel.transform.Find("Pitch").gameObject.SetActive(true);
        camPanel.transform.Find("Yaw").gameObject.SetActive(true);
        camPanel.transform.Find("Speed").gameObject.SetActive(true);
        camPanel.transform.Find("Backward").gameObject.SetActive(true);
        camPanel.transform.Find("Forward").gameObject.SetActive(true);
    }

    void SetDifficultyHard()
    {
        ColorBlock cb = camPanel.transform.Find("Hard").GetComponent<Button>().colors;
        cb.normalColor = highlightColor;
        camPanel.transform.Find("Hard").GetComponent<Button>().colors = cb;

        cb = camPanel.transform.Find("Easy").GetComponent<Button>().colors;
        cb.normalColor = Color.white;
        camPanel.transform.Find("Easy").GetComponent<Button>().colors = cb;

        foreach (GameObject tray in GameObject.FindGameObjectsWithTag("Tray"))
        {
            tray.GetComponent<TrayControl>().difficulty = "hard";
        }
    }

    void SetDifficultyEasy()
    {
        ColorBlock cb = camPanel.transform.Find("Easy").GetComponent<Button>().colors;
        cb.normalColor = highlightColor;
        camPanel.transform.Find("Easy").GetComponent<Button>().colors = cb;

        cb = camPanel.transform.Find("Hard").GetComponent<Button>().colors;
        cb.normalColor = Color.white;
        camPanel.transform.Find("Hard").GetComponent<Button>().colors = cb;

        foreach (GameObject tray in GameObject.FindGameObjectsWithTag("Tray")) {
            tray.GetComponent<TrayControl>().difficulty = "easy";
        }
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

    void DeactivateBeltPanel()
    {
        KaitenController ctrl = GameObject.Find("Kaiten Zushi").GetComponent<KaitenController>();
        beltPanel.SetActive(false);
        ctrl.belt_mat.color = Color.gray;
        ctrl.beltSpeed = ctrl.prevSpeed;
        GameObject.Find("GameState").GetComponent<GameState>().paused = false;
        WakeUpPlates();
    }

    void CloseWindow()
    {
        KaitenController ctrl = GameObject.Find("Kaiten Zushi").GetComponent<KaitenController>();

        if (beltPanel.activeInHierarchy)
        {
            DeactivateBeltPanel();
        }
        else if (saucePanel.activeInHierarchy)
        {
            saucePanel.SetActive(false);
        }
        else
        {
            camPanel.SetActive(false);
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
                    //Debug.Log("Selected sauce");
                    saucePanel.SetActive(true);
                    DeactivateBeltPanel();
                    //beltPanel.SetActive(false);
                    camPanel.SetActive(false);

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

        KaitenController ctrl = GameObject.Find("Kaiten Zushi").GetComponent<KaitenController>();
        if (beltPanel.activeInHierarchy)
            beltPanel.transform.Find("Slider").GetComponent<Slider>().value = ctrl.prevSpeed;
            //GameObject.Find("Value").GetComponent<Text>().text = GameObject.Find("Kaiten Zushi").GetComponent<KaitenController>().prevSpeed.ToString();

    }
}
