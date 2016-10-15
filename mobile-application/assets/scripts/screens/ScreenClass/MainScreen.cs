using UnityEngine;
using System.Collections;
using Amigosi.Screen;
using UnityEngine.UI;
using Amigosi.Server;
using System;
using System.Collections.Specialized;

public class MainScreen : Amigosi.Screen.Screen {
    public Animator emerAnimator; 

    public void BankButton()
    {
        googleMaps.findLocation("bank");
    }

    public void ATMButton()
    {
        googleMaps.findLocation("atm");
    }

    public void shelterButton()
    {
        screenManager.StartScreen("ShelterScreen");
    }

    public void hospitalsButton()
    {
        googleMaps.findLocation("hospital");
    }

    public void restouratButton()
    {
        googleMaps.findLocation("food");
    }

    public void converterButton()
    {
        screenManager.StartScreen("ConverterScreen");
    }

    public void ShowEmer()
    {
        emerAnimator.SetTrigger("change");
    }
    public void callHospital()
    {
        callEmergency("194");
    }
    public void callPolice()
    {
        callEmergency("192");
    }
    public void callEmergency(string varr)
    {
        Application.OpenURL("tel://"+varr);
    }

    public void startQRCODEScreen()
    {
        screenManager.StartScreen("BarCodeScreen");
    }

    public Text viewIDButton;
    public GoogleMapsCaller googleMaps;

    void OnEnable()
    {
        viewIDButton.text = user.getViewID();
        NameValueCollection collection = new NameValueCollection();
        collection.Add("imei", user.getIMEI());
        collection.Add("longitude", Convert.ToString(user.getLongit()));
        collection.Add("latitude", Convert.ToString(user.getLatit()));
        collection.Add("city", " ");
        collection.Add("gps", "1");
        WebRequest webReq = new WebRequest("", "http://10.66.47.75/v1/user/location/set", collection);
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
                user.setViewIDJSON(webReq.data);
                screenManager.StartScreen("MainScreen");
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

    }
