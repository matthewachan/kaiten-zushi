using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    ArrayList plates;

    // Start is called before the first frame update
    void Start()
    {
        plates = new ArrayList();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Plate" && !plates.Contains(other) && !other.GetComponent<SushiController>().dragging)
        {
            if (plates.Count == 4)
                other.GetComponent<SushiController>().Break();
            else
                plates.Add(other.gameObject);
            Debug.Log("Table has " + plates.Count + " plates on it!");

        }
    }



    // Update is called once per frame
    void Update()
    {

        // Check if a plate has been consumed
        ArrayList toRemove = new ArrayList();
        foreach (GameObject plate in plates)
        {
            if (!plate)
            {
                toRemove.Add(plate);
            }
        }
        foreach (GameObject plate in toRemove)
            plates.Remove(plate);
    }
}
