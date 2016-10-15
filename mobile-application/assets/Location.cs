using UnityEngine;
using System.Collections;

public class Location : MonoBehaviour {
    public GoogleMapsCaller googleMaps;
    public string longitude;
    public string latitude;
    void Start()
    {
        longitude = "";
        latitude = "";
    }
    public void AddButton()
    {
        //"http://maps.googleapis.com/maps/api/geocode/json?latlng="
        //    44.4647452,7.3553838&sensor=true"
        googleMaps.findLocWithLL(longitude,latitude);
    }
}
