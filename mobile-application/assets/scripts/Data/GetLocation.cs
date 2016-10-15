using UnityEngine;
using System.Collections;
using Amigosi.Data;

public class GetLocation : MonoBehaviour
{
    private float longi, lati;
    public User user;
    // If gps is enabled this func. starts google maps app or url to search for things added by attributes
    IEnumerator Start()
    {
        // Is location ervice enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Disabled GPS");
            yield break;
        }
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
        {            
            lati = Input.location.lastData.latitude;
            longi = Input.location.lastData.longitude;
            Debug.Log("latitude" + lati);
            Debug.Log("longitude" + longi);
            user.setLongLat(longi, lati);
        }
        // Stop service if you don't need to check location all the time
        // !!! Stops battery drain
        Input.location.Stop();
    }
}
