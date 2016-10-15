using SimpleJSON;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Amigosi.Server;
using System.Collections.Specialized;

namespace Amigosi.Data
{
    /*
    Ova klasa je za cuvanje glavnih podataka o korisniku.
    User data class */
    public class User : MonoBehaviour
    {
        #region  User data atributes    
        private string IMEI;
        private string viewID;
        private string fromCountry;
        private string destinationCountry;
        private string numChilder;
        private string numAdults;
        private float longitudeFirstSeen, latitudeFirstSeen;
        #endregion

        #region User data methods
        void Awake()
        {
            viewID = "";
            fromCountry = "";
            destinationCountry = "";
            numChilder = "";
            numAdults = "";
            longitudeFirstSeen = 0;
            latitudeFirstSeen = 0;
            loadIMEI();

            NameValueCollection collection = new NameValueCollection();
            collection.Add("imei", IMEI);
            WebRequest webReq = new WebRequest("", "http://10.66.47.75/v1/auth/check", collection);
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
                    setViewIDJSON(webReq.data);
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


        public void setLongLat(float longitudeFirstSeen, float lat)
        {
            this.latitudeFirstSeen = lat;
            this.longitudeFirstSeen = longitudeFirstSeen;
        }
        public void loadIMEI()
        {
            IMEI = SystemInfo.deviceUniqueIdentifier;
            Debug.Log("IMEI: " + IMEI);
        }
        public string getIMEI()
        {
            return IMEI;
        }
        public string getViewID()
        {
            return viewID;
        }
        public void setNumChilder(string child)
        {
            numChilder = child;
        }
        public void setNumAdults(string numAdults)
        {
            this.numAdults = numAdults;
        }
        public string getNumChild()
        {
            return numChilder;
        }
        public string getNumAdults()
        {
            return numAdults;
        }
        public void setDestination(string to)
        {
            this.destinationCountry = to;
        }
        public void setFrom(string from)
        {
            this.fromCountry = from;
        }
        public string getFromCountry()
        {
            return fromCountry;
        }
        public string getDestinationCountry()
        {
            return destinationCountry;
        }

        public float getLongit()
        {
            return longitudeFirstSeen;
        }
        public float getLatit()
        {
            return latitudeFirstSeen;
        }

        //Check post status
        public bool PostStatusCheck(string data)
        {
            JSONNode jsonObject = JSON.Parse(data);
            if (jsonObject["status"].Value == "200")
                return true;
            return false;
        }

        //Login parser
        public void setViewIDJSON(string data)
        {
            JSONNode jsonObject = JSON.Parse(data);
            string ret = jsonObject["data"].Value;
            if (ret != string.Empty)
                viewID = ret;
        }


        #endregion
    }
}