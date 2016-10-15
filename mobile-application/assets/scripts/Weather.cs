using UnityEngine;
using System.Collections;
using Amigosi.Screen;
using UnityEngine.UI;
using Amigosi.Server;
using System;
using Amigosi.Data;
using SimpleJSON;
using System.Collections.Specialized;

public class Weather : MonoBehaviour {

    public User user;

	void Start () {
        NameValueCollection collection = new NameValueCollection();
        collection.Add("imei", "e2a95547d9a2e9d525a8ea7530ed102e");//user.getIMEI());
        WebRequest webReq = new WebRequest("", "http://10.66.47.75/v1/user/location/weather", collection);
        webReq.POSTRequest();
        StartCoroutine(RegisterReq(webReq));
    }

    IEnumerator RegisterReq(WebRequest webReq)
    {
        while (!webReq.POSTcomplete && webReq.busy)
        {
            yield return new WaitForSeconds(0.5f);
        }

        if (webReq.data != string.Empty)
        {
            if (SimpleJSON.JSON.Parse(webReq.data)["status"].Value == "200")
            {
                parseJSON(webReq.data);
            }
            else
            {
                // errorText.text = "Wrong password or email.";
            }
        }
        else
        {
            //errorText.text = "No network connection.";
        }

    }

    public void parseJSON(string data)
    {
        string HOLETEMP = "";
        JSONNode jsonObject = JSON.Parse(data);
        JSONNode obj0 = jsonObject["data"].AsObject;
        //Description
        JSONNode obj2 = obj0["weather"].AsObject;
        string sdesc = jsonObject["weather"].AsArray["description"].Value;

        //HOLETEMP += " " + obj2Array["main"] + " , ";
        
        //Temp
        JSONNode obj = obj0["main"].AsObject;
        string tmp = obj["temp"];
        double c = Convert.ToDouble(tmp) - 273.0;

        HOLETEMP += sdesc + " " + c.ToString() + " degrees Celsius!";
    }

}
