using UnityEngine;
using System.Collections;
using Amigosi.Screen;
using System.Collections.Generic;
using Amigosi.Server;
using System.Collections.Specialized;
using UnityEngine.UI;

public class ShelterScreen : Amigosi.Screen.Screen
{
    public GameObject instanca;
    public GameObject parent;
    public List<Shelter> list;
    
    void Awake()
    {
       // list = new List<Shelter>();
    }
    void OnEnable()
    {
        list = new List<Shelter>();
    
        
        NameValueCollection data = new NameValueCollection();
        data.Add("imei", user.getIMEI());
        WebRequest req = new WebRequest("", "http://10.66.47.75/v1/camps/nearby",data);
        req.POSTRequest();
        StartCoroutine(DownloadList(req));
    }

    IEnumerator DownloadList(WebRequest wr)
    {
        while (!wr.POSTcomplete && wr.busy)
        {
            yield return new WaitForSeconds(0.5f);
        }

        if (wr.data != string.Empty)
        {
            if (SimpleJSON.JSON.Parse(wr.data)["status"].Value == "200")
            {
                var data = SimpleJSON.JSON.Parse(wr.data);

                SimpleJSON.JSONArray j = data["data"].AsArray;
                foreach (SimpleJSON.JSONNode n in j)
                {
                    list.Add(new Shelter(n["id"].Value, n["name"].Value, n["location"].Value, n["longitude"].Value, n["latitude"].Value, n["distance"].Value, n["units"].Value, Instantiate(instanca),n["numbergoingto"].Value, n["numberthere"].Value));
                    //list[n].instance.transform.GetComponent<Location>().latitude = n["latitude"].Value;
                    //list[n].instance.transform.GetComponent<Location>().longitude = n["longitude"].Value;

                }
                foreach (Shelter s in list)
                {
                    s.instance.transform.SetParent(parent.transform);
                    s.instance.transform.localScale = Vector3.one;
                    Text[] text=s.instance.transform.GetComponentsInChildren<Text>();
                    text[0].text = s.name;
                    text[1].text = "Location:"+s.location;
                    text[2].text = "Dis.:"+s.distance+" "+s.units+"\n("+s.numbergoingto+"/"+s.numberthere+") on way/in";
                    //s.instance.transform.GetComponent<Location>().latitude = s.latitude;
                    //s.instance.transform.GetComponent<Location>().longitude = s.longitude;
                }
            }
        }

    }
    public void BackButton()
    {
        if (list.Count > 0)
        {
            foreach (Shelter s in list)
            {
                Destroy(s.instance);
            }
        }
        screenManager.StartScreen("MainScreen");
    }
}
public class Shelter
{
    public string id;
    public string name;
    public string location;
    public string longitude;
    public string latitude;
    public string distance;
    public string units;
    public GameObject instance;
    public string numbergoingto;
    public string numberthere;
    public Shelter(string id_,string name_,string location_,string longitude_,string latitude_,string distance_,string units_,GameObject ins,string numbergt,string numbert)
    {
        id = id_;
        name = name_;
        location = location_;
        longitude = longitude_;
        distance = distance_;
        latitude = latitude_;
        units = units_;
        instance = ins;
        numbergoingto = numbergt;
        numberthere = numbert;
    }
}

