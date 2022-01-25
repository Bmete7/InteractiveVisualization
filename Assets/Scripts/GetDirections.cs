using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GetDirections : MonoBehaviour
{
    public bool toggleButton;
    public GameObject arrow;
    public GameObject distanceButton;
    public Button myButton;
    

    public float myLatitude;
    public float myLongitude;
    public float horseRiderLatitude;
    public float horseRiderLongitude;
    private float[,] landmarkCoordinates;

    // Start is called before the first frame update
    void Start()
    {
        toggleButton = false;
        myButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if (toggleButton == false)
        {
            toggleButton = true;
            arrow.SetActive(true);
            distanceButton.SetActive(true);

        }
        else
        {
            toggleButton = false;
            arrow.SetActive(false);
            distanceButton.SetActive(false);
            
        }
    }
}
