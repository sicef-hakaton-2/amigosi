using UnityEngine;
using System.Collections;
using Amigosi.Screen;
using UnityEngine.UI;
using Amigosi.Server;
using System;
using System.Collections.Specialized;
using SimpleJSON;

public class RegistrationScreen : Amigosi.Screen.Screen
{
    public InputField numberFamaly, numberChildren;
    public Dropdown finalDestination, fromDestination;

    void Start()
    {
        for (int i = 0; i < appData.listaZemalja.Count; ++i)
            finalDestination.options.Add(new Dropdown.OptionData(appData.listaZemalja[i].ToString()));

        for(int i = 0; i < appData.fromZemlje.Count; ++i)
            fromDestination.options.Add(new Dropdown.OptionData(appData.fromZemlje[i].ToString()));
    }

    public void proceedButton()
    {
        bool greska = false;
        if (numberFamaly.text == "")
        {
            numberFamaly.text = "*You missed this!";
            greska = true;
        }
        if (numberChildren.text == "")
        {
            numberChildren.text = "*You missed this!";
            greska = true;
        }

        if (greska)
            return;
     
      
        user.setNumAdults(numberFamaly.text);
        user.setNumChilder(numberChildren.text);
        user.setDestination(finalDestination.captionText.text);
        user.setFrom(fromDestination.captionText.text);

        user.setLongLat(1, 1);

        NameValueCollection collection = new NameValueCollection();
        collection.Add("imei", user.getIMEI());
        collection.Add("countryfrom",user.getFromCountry().Substring(0, 2));
        collection.Add("countryto", user.getDestinationCountry().Substring(0, 2));
        collection.Add("numchildren", user.getNumChild().ToString());
        collection.Add("numadults", user.getNumAdults().ToString());
        WebRequest webReq = new WebRequest("", "http://10.66.47.75/v1/auth/register", collection);
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
            if (JSON.Parse(webReq.data)["status"].Value == "200")
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

    public void cancelButton()
    {
        screenManager.StartScreen("StartScreen");
    }

}
