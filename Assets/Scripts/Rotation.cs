using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Rotation : MonoBehaviour
{
    public float myLatitude;
    public float myLongitude;
    public float horseRiderLatitude;
    public float horseRiderLongitude;
    private float[,] landmarkCoordinates;
    [SerializeField] TextMeshProUGUI myText;
    // Start is called before the first frame update
    void Start()
    {
        Input.compass.enabled = true;
        myLatitude = Input.location.lastData.latitude;
        myLongitude = Input.location.lastData.longitude;
        float[] myCoordinates = new float[2];
        myCoordinates[0] = myLatitude;
        myCoordinates[1] = myLongitude;
        horseRiderLatitude = 140;
        horseRiderLongitude = 40;
        int numberOfLandmarks = 5;
        landmarkCoordinates = new float[numberOfLandmarks, 2];
        for(int i = 0; i < numberOfLandmarks; ++i)
        {
            landmarkCoordinates[i, 0] = 48.14854074f; // latitude of the ith landmark
            landmarkCoordinates[i, 1] = 11.56905809f;
            // landmarkCoordinates[i, 0] = 48.20054074f; // latitude of the ith landmark
            // landmarkCoordinates[i, 1] = 11.62705809f;
        }
        // coordinates for the Rossebaediger 48.14854074528244, 11.56905809258522

        float y = (myLongitude - horseRiderLongitude) / 360;
        float x = (myLatitude - horseRiderLatitude) / 180;
        float[] currentLandmarkCoordinates = new float[2];
        currentLandmarkCoordinates[0] = landmarkCoordinates[0, 0];
        currentLandmarkCoordinates[1] = landmarkCoordinates[0, 1];
        float bearing = Bearing(myCoordinates, currentLandmarkCoordinates);
        //transform.Rotate(0, 0,Mathf.Atan(y / x) * (180 / Mathf.PI));
        // transform.Rotate(0, 0, bearing + Input.gyro.attitude[2]);
        //transform.Rotate(0, 0, Input.gyro.attitude[2]);
        transform.Rotate(0, 0, Input.compass.trueHeading);
        transform.Rotate(0, 0, bearing);
    }

    // Update is called once per frame
    void Update()
    {

        myLatitude = Input.location.lastData.latitude;
        myLongitude = Input.location.lastData.longitude;
        float[] currentLandmarkCoordinates = new float[2];
        currentLandmarkCoordinates[0] = landmarkCoordinates[0, 0];
        currentLandmarkCoordinates[1] = landmarkCoordinates[0, 1];
        float[] myCoordinates = new float[2];
        myCoordinates[0] = myLatitude;
        myCoordinates[1] = myLongitude;

        float distance = DistanceCalculation(myCoordinates, currentLandmarkCoordinates);
        float bearing = Bearing(myCoordinates, currentLandmarkCoordinates);
        transform.Rotate(0, 0, -1 * Input.gyro.rotationRate[2] );
        
        myText.text = distance.ToString() + " m";
        //transform.Rotate(Input.gyro.rotationRate);
        
    }

    public static float Bearing(float[] pt1, float[] pt2)
    {
    
        float x = Mathf.Cos(DegreesToRadians(pt1[0])) * Mathf.Sin(DegreesToRadians(pt2[0])) - Mathf.Sin(DegreesToRadians(pt1[0])) * Mathf.Cos(DegreesToRadians(pt2[0])) * Mathf.Cos(DegreesToRadians(pt2[1] - pt1[1]));
        float y = Mathf.Sin(DegreesToRadians(pt2[1] - pt1[1])) * Mathf.Cos(DegreesToRadians(pt2[0]));

        // Math.Atan2 can return negative value, 0 <= output value < 2*PI expected 
        return (Mathf.Atan2(y, x) + Mathf.PI * 2) % (Mathf.PI * 2);
    }

    public static float DegreesToRadians(float angle)
    {
        return angle * Mathf.PI / 180.0f;
    }

    public static float DistanceCalculation(float[] pt1, float[] pt2)
    {
        
        float R = 6371000; // metres
        float d1 = pt1[0] * Mathf.PI / 180; 
        float d2 = pt2[0] * Mathf.PI / 180;
        float d3 = (pt2[0] - pt1[0]) * Mathf.PI / 180;
        float d4 = (pt2[1] - pt1[1]) * Mathf.PI / 180;

        float a = Mathf.Sin(d3 / 2) * Mathf.Sin(d3 / 2) + Mathf.Cos(d1) * Mathf.Cos(d2) * Mathf.Sin(d4 / 2) * Mathf.Sin(d4 / 2);
        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));

        float distance = R * c; // in metres
        


        /*float distance = new Coordinates(pt1[0], pt1[1])
                .DistanceTo(
                    new Coordinates(pt2[0], pt2[1]),
                    UnitOfLength.Kilometers
                );
                */
        return distance;
    }

}

public class Coordinates
{
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public Coordinates(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}
public static class CoordinatesDistanceExtensions
{
    public static float DistanceTo(this Coordinates baseCoordinates, Coordinates targetCoordinates)
    {
        return DistanceTo(baseCoordinates, targetCoordinates, UnitOfLength.Kilometers);
    }

    public static float DistanceTo(this Coordinates baseCoordinates, Coordinates targetCoordinates, UnitOfLength unitOfLength)
    {
        float baseRad = ToSingle(Mathf.PI * baseCoordinates.Latitude / 180);
        
        float targetRad = ToSingle(Mathf.PI * targetCoordinates.Latitude / 180);
        float theta = ToSingle(baseCoordinates.Longitude - targetCoordinates.Longitude);
        float thetaRad = Mathf.PI * theta / 180;

        float dist = Mathf.Sin(baseRad) * Mathf.Sin(targetRad) + Mathf.Cos(baseRad) * Mathf.Cos(targetRad) * Mathf.Cos(thetaRad);
        dist = Mathf.Acos(dist);

        dist = dist * 180 / Mathf.PI;
        dist = dist * 60 * 1.1515f;

        return unitOfLength.ConvertFromMiles(dist);
    }
    public static float ToSingle(double value)
    {
        return (float)value;
    }
}

public class UnitOfLength
{
    public static UnitOfLength Kilometers = new UnitOfLength(1.609344f);
    public static UnitOfLength NauticalMiles = new UnitOfLength(0.8684f);
    public static UnitOfLength Miles = new UnitOfLength(1);

    private readonly float _fromMilesFactor;

    private UnitOfLength(float fromMilesFactor)
    {
        _fromMilesFactor = fromMilesFactor;
    }

    public float ConvertFromMiles(float input)
    {
        return input * _fromMilesFactor;
    }
}

