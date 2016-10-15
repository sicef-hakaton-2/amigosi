using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using Amigosi.Server;
using System.Collections.Specialized;
using System;
using Amigosi.Data;
using UnityEngine.UI;
using Amigosi.Screen;

public class QRCodeDownloader : Amigosi.Screen.Screen, IDownloadStatus
{

    public GameObject obj;
    public User user;
    private ServerAssetsDownloader server;
    private string filePath;
    private bool isDownloadFinished;
    private Amigosi.Server.WebRequest webReq;
    // Use this for initialization
    void Start()
    {
        isDownloadFinished = false;
        if (File.Exists(Application.persistentDataPath + "DownloadedImages" + "/" + "qr.png"))
        {
            
            var filename = Application.persistentDataPath + "DownloadedImages" + "/" + "qr.png";
            var bytes = File.ReadAllBytes(filename);
            var texture = new Texture2D(150, 150);
            texture.LoadImage(bytes);
            Sprite s = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100.0f);
            obj.GetComponent<Image>().overrideSprite = s;
            filePath = "";
            isDownloadFinished = false;
        }
        else
        {
            filePath = "";
            isDownloadFinished = false;
            server = new ServerAssetsDownloader(this);

            NameValueCollection collection = new NameValueCollection();
            collection.Add("imei", user.getIMEI());
            collection.Add("longitude", Convert.ToString(user.getLongit()));
            collection.Add("latitude", Convert.ToString(user.getLatit()));
            collection.Add("city", " ");
            collection.Add("gps", "1");
            webReq = new Amigosi.Server.WebRequest("", "http://10.66.47.75/v1/other/qrgen", collection);
            webReq.POSTRequest();
            StartCoroutine(RegisterReq(webReq));
        }
    }

    IEnumerator RegisterReq(Amigosi.Server.WebRequest webReq)
    {
        while (!webReq.POSTcomplete && webReq.busy)
        {
            yield return new WaitForSeconds(0.5f);
        }

        if (webReq.data != string.Empty)
        {
            if (SimpleJSON.JSON.Parse(webReq.data)["status"].Value == "200")
            {
                string json = SimpleJSON.JSON.Parse(webReq.data)["data"].Value;
                server.downloadFile(json, "qr.png");
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

    public void downloadFinished(string filePath)
    {
        Debug.Log("Download finished!");
        this.filePath = filePath;
        isDownloadFinished = true;
    }

    public void downloadFaild()
    {
        Debug.Log("Download faild, some error occured!");
        isDownloadFinished = false;
    }

    void Update()
    {
        if (isDownloadFinished)
        {
            var filename = Application.persistentDataPath + "DownloadedImages" + "/" + "qr.png";
            var bytes = File.ReadAllBytes(filename);
            var texture = new Texture2D(150, 150);
            texture.LoadImage(bytes);
            Sprite s = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100.0f);
            obj.GetComponent<Image>().overrideSprite = s;
            isDownloadFinished = false;
            this.filePath = "";
        }

    }

    public void BackButton()
    {
        screenManager.StartScreen("MainScreen");
    }
}
