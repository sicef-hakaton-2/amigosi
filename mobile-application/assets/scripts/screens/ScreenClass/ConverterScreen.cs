using UnityEngine;
using System.Collections;
using Amigosi.Screen;
using SimpleJSON;
using UnityEngine.UI;
using Amigosi.Server;
using System;

using System.Collections.Specialized;
using System.Collections.Generic;

public class ConverterScreen : Amigosi.Screen.Screen
{

    public InputField unos, valutaU;
    public Dropdown valutaIz;

    private List<string> srednjaVrednostKursa;
    void OnEnable()
    {
        unos.text = string.Empty;
        valutaU.text = string.Empty;
    }
    void Start()
    {
        srednjaVrednostKursa = new List<string>();
        NameValueCollection collection = new NameValueCollection();
        collection.Add("imei", "f2aba239b5f722cd0733e69c22be9eb1bb61237c");
        WebRequest webReq = new WebRequest("", "http://10.66.47.75/v1/other/currency", collection);
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

    //Login parser
    public void parseJSON(string data)
    {
        JSONNode jsonObject = JSON.Parse(data);
        JSONNode obj = jsonObject["data"].AsObject;
        srednjaVrednostKursa.Add(obj["eur"].Value);
        srednjaVrednostKursa.Add(obj["usd"].Value);
        srednjaVrednostKursa.Add(obj["chf"].Value);
        srednjaVrednostKursa.Add(obj["gbp"].Value);
    }

    public void changeFieldTxt1()
    {

        decimal value = Convert.ToDecimal(unos.text);
        int izSelected = valutaIz.value;
        decimal srednjiKurs = Convert.ToDecimal(srednjaVrednostKursa[izSelected]);
        decimal finalValue = value / srednjiKurs;
        valutaU.text = finalValue.ToString();
    }


    public void changeFieldTxt2()
    {

        decimal value = Convert.ToDecimal(valutaU.text);
        int izSelected = valutaIz.value;
        decimal srednjiKurs = Convert.ToDecimal(srednjaVrednostKursa[izSelected]);
        decimal finalValue = value * srednjiKurs;
        unos.text = finalValue.ToString();
    }

    public void BackButton()
    {
        screenManager.StartScreen("MainScreen");
    }

    /*
    public void changeField()
    {
        if(unos.text != "" && valutaU.text == "")
        { 
            decimal value = Convert.ToDecimal(unos.text);
            int izSelected = valutaIz.value;
            decimal srednjiKurs = Convert.ToDecimal(srednjaVrednostKursa[izSelected]);
            decimal finalValue = value / srednjiKurs;
            valutaU.text = finalValue.ToString();
        }
        else if(valutaU.text != "" && unos.text == "")
        {
            decimal value = Convert.ToDecimal(valutaU.text);
            int izSelected = valutaIz.value;
            decimal srednjiKurs = Convert.ToDecimal(srednjaVrednostKursa[izSelected]);
            decimal finalValue = value * srednjiKurs;
            unos.text = finalValue.ToString();
        }
    }*/
}
