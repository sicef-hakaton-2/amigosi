using UnityEngine;
using System.Collections;
using Amigosi.Data;

//Nemanja Petrovic
public class GoogleMapsCaller : MonoBehaviour
{

    // Call this function to start coroutine
    public void findLocation(string attributes)
    {       
        StartCoroutine(LocationService(attributes));
    }

    // If gps is enabled this func. starts google maps app or url to search for things added by attributes
    IEnumerator LocationService(string attributes)
    {
        // Is location ervice enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service
        Input.location.Start();

        // Wait initialization
        int timeForInit = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && timeForInit > 0)
        {
            yield return new WaitForSeconds(1);
            timeForInit--;
        }

        // Service didn't initialize in 20 seconds
        if (timeForInit < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else        
            Application.OpenURL("https://www.google.com/maps/search/" + attributes + "/@" + Input.location.lastData.latitude + "," + Input.location.lastData.longitude + ",14z/data=!3m1!4b1?hl=sr");                

        // Stop service if you don't need to check location all the time
        // !!! Stops battery drain
        Input.location.Stop();
    }

    public void findLocWithLL(string longitude,string latitude)
    {
        StartCoroutine(LocationService2( longitude,  latitude));
    }

    IEnumerator LocationService2(string longitude, string latitude)
    {
        // Is location ervice enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service
        Input.location.Start();

        // Wait initialization
        int timeForInit = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && timeForInit > 0)
        {
            yield return new WaitForSeconds(1);
            timeForInit--;
        }

        // Service didn't initialize in 20 seconds
        if (timeForInit < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else
            Application.OpenURL("https://www.google.com/maps/search/" + "" + "/@" + latitude + "," + longitude + ",14z/data=!3m1!4b1?hl=sr");

        // Stop service if you don't need to check location all the time
        // !!! Stops battery drain
        Input.location.Stop();
    }
}




