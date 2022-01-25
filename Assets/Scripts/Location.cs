using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Location : MonoBehaviour
{


    // audioSource = GetComponent<AudioSource>(); For audio guide
    public Text GPSStatus;
    public Text latitudeValue;
    public Text longitudeValue;
    public Text altitudeValue;
    public Text horizontalAccuracyValue;
    public Text timestamp;
    public double horseRiderLatitude;
    public double horseRiderLongitude;
    public double myLatitude;
    public double myLongitude;
    public GameObject myObject;
    public Quaternion targetDirection;
    
    private GameObject cameraContainer;
    private Quaternion rot;

    
    Vector3 dir;



    // Start is called before the first frame update
    void Start()
    {
        
        Input.location.Start();
        Input.compass.enabled = true;
        Input.gyro.enabled = true;

        StartCoroutine(GPSLoc());
    }

    // Update is called once per frame
    IEnumerator GPSLoc()
    {
        if (!Input.location.isEnabledByUser)
        {
            
            //yield break;
        }
            
        Input.location.Start();
        int maxWait = 20;
        while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        if(maxWait < 1)
        {
            
            GPSStatus.text = "timeout";
            yield break;
        }
        // connection failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {

            GPSStatus.text = "unable to determine device location";
            yield break;

        }
        else
        {

            InvokeRepeating("UpdateGPSData", 0.5f, 1f);
            //access granted
        }
    } // end of gps loc

    private void UpdateGPSData()
    {
        if(Input.location.status == LocationServiceStatus.Running)
        {
            
            
            // access granted and has been init
            
            myLatitude = Input.location.lastData.latitude;
            myLongitude = Input.location.lastData.longitude;

            horseRiderLatitude = 20;
            horseRiderLongitude = 10;

            
            //latitudeValue.text = Input.gyro.rotationRate.ToString();
            //longitudeValue.text = Input.gyro.attitude.ToString();
            //latitudeValue.text = " a";
            
            ChangeDirection();
            

            
        }
        else
        {
            //.text = Input.location.lastData.latitude.ToString();
            //longitudeValue.text = Input.location.lastData.longitude.ToString();
            Input.location.Start(1, 0.1f);
        } // service is stopped
    }

    public void ChangeDirection()
    {

        
    }

}
