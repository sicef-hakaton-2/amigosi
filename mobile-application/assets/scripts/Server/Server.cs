using UnityEngine;
using System.Net;
using System;
using System.ComponentModel;
using System.Text;
using System.IO;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections;

namespace Amigosi.Server
{
    #region ServerAssetsDownloader
    [Serializable]    
    public class ServerAssetsDownloader
    {

        //Root directory
        private static string rootFolderPath = Application.persistentDataPath;
        //Folder name
        private const string folderName = "DownloadedImages";

        //Interface IDownloadStatus
        private IDownloadStatus iDownloadStatus;

        //Constructor
        public ServerAssetsDownloader(IDownloadStatus iDownloadStatus)
        {
            createDownloadDirectory();
            this.iDownloadStatus = iDownloadStatus;
        }

        //Downloading file from url with filename, and sending gameobject into event to set it's components
        public void downloadFile(string url, string fileName)
        {
            string path = rootFolderPath + folderName + "/" + fileName;
            Debug.Log("Saving path with image: " + path);
            Uri urlURI = new Uri(url);
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFileAsync(urlURI, path);
                    wc.DownloadFileCompleted += new AsyncCompletedEventHandler((s, e) => setGameObjectComponent(s, e, path));
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        //Setting gameobject componet
        private void setGameObjectComponent(object sender, System.ComponentModel.AsyncCompletedEventArgs e, string imgPath)
        {
            if (iDownloadStatus != null)
                iDownloadStatus.downloadFinished(imgPath);
            else
                iDownloadStatus.downloadFaild();
        }

        //Creating directory
        private void createDownloadDirectory()
        {
            try
            {
                if (!Directory.Exists(rootFolderPath + folderName))
                {
                    Directory.CreateDirectory(rootFolderPath + folderName);
                    Debug.Log("Creating folder: " + (rootFolderPath + folderName));
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }        
    }
    #endregion
    #region FileStream
    [Serializable]
    public class FileStream
    {
        public int ID;
        public List<FileDownloadSession> downloadList;
        //Staticka putanja za fajlove u zavisnosti od platforme
        public static string localDir;
        public FileStream()
        {
            downloadList = new List<FileDownloadSession>();
            ID = 0;
            localDir = Application.persistentDataPath;
        }
        //Startuje novi download task
        public void StartNewDownload(string fileUrl, string localPath)
        {
            FileDownloadSession newS = new FileDownloadSession(ID, fileUrl, localPath/*+localDir*/);
            downloadList.Add(newS);
            newS.FileDownload();
            ID++;

        }
        //Metoda cisti listu ako su sve sesije zavrsile download
        public void ClearConnections()
        {
            int completed = 0;
            for (int i = 0; i < downloadList.Count; i++)
            {
                if (downloadList[i].complete)
                    completed++;
            }
            if (downloadList.Count == completed)
            {
                downloadList.Clear();
                ID = 0;
            }
        }
        //Metoda za brisanje skinuti fajlova ,brisati tek ako si siguran da je fajl skinut za uspesno brisanje
        //Parametar je nastavak iza localDir pr:  localDir/privremeniFajlovi
        public void DeleteDownlodesFolder(string folderName)
        {
            try
            {
                Directory.Delete(localDir + folderName);
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.Log("Delete error: " + ex.Message);
            }
            catch (DirectoryNotFoundException ex)
            {
                Debug.Log(ex.Message);
            }
            catch (IOException ex)
            {
                Debug.Log(ex.Message);
            }
        }
        public void CreateDownloadFolder(string folderName)
        {
            if (!Directory.Exists(localDir + folderName)) 
            Directory.CreateDirectory(localDir + folderName);
        }
    }
    //Sesija za skidanje 
    [Serializable]
    public class FileDownloadSession
    {
        //Ovde ako treba sve treba ode u private i da se naprave seteri i geteri
        //Svi podatci o downloadu
        public long id;
        WebClient client;
        public string fileUrl;
        public string localPath;
        public bool complete;
        public bool busy;
        public int progress;
        //Osnovno konstruktor
        public FileDownloadSession(long _id, string _fileUrl, string _localPath)
        {
            id = _id;
            client = new WebClient();
            fileUrl = _fileUrl;
            localPath = _localPath;
            complete = false;
            busy = false;
            progress = 0;
        }

        //Metoda za download
        public void FileDownload()
        {
            //Proverava dali je zauzeta sesija
            if (busy)
                return;
            else
                busy = true;
            //inicijalizacija za slucaj ponovnog slanja u istoj sesiji
            complete = false;
            progress = 0;
            try
            {
                /*if(!File.Exists(localPath))
                    throw new DirectoryNotFoundException();*/
                Uri uri = new Uri(fileUrl);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(FileDownloadComeplete);
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(FileDownloadProgress);
                client.DownloadFileAsync(uri, localPath);
            }
            catch (ArgumentNullException ex)
            {
                Debug.Log(ex.Message);
            }
            catch (WebException ex)
            {
                Debug.Log(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Debug.Log(ex.Message);
            }
            catch (DirectoryNotFoundException ex)
            {
                Debug.Log(ex.Message);
            }

        }

        //Callback metoda za proveravanje dali je fajl skinut
        private void FileDownloadComeplete(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            busy = false;
            if (e == null || e.Error != null)
            {
                throw new Exception("Download " + id + " error.");
            }
            else if (e.Cancelled)
                throw new Exception("Download " + id + " cancelled.");
            else
                complete = true;
        }
        //Callback metoda za proveravanje progresa
        private void FileDownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            progress = e.ProgressPercentage;
        }
    }
#endregion
    #region WebRequest
    [Serializable]
    public class WebRequest
    {
        private WebClient client;
        private string url;
        public string senderId;
        private NameValueCollection POSTdata;
        public string data;
        public bool POSTcomplete;
        public bool busy;

        public WebRequest(string _senderId, string _serverUrl, NameValueCollection _data)
        {
            client = new WebClient();
            url = _serverUrl;
            senderId = _senderId;
            POSTdata = _data;
            data = string.Empty;
            POSTcomplete = false;
            busy = false;
        }
        public void POSTRequest()//NameValueCollection je kolekcija kljuceva i vrednosti formata "key" "value"
        {
            //Proverava dali je zauzeta sesija
            if (busy)
                return;
            else
                busy = true;
            //Inicijalizacija za slucaj ponovnog slanja istog requesta
            POSTcomplete = false;

            try
            {
                Uri uri = new Uri(url);
                client.UploadValuesCompleted += new UploadValuesCompletedEventHandler(POSTRequestCompleted);
                client.UploadValuesAsync(uri, POSTdata);
            }
            catch (ArgumentNullException ex)
            {
                Debug.Log(ex.Message);
            }
            catch (WebException ex)
            {
                Debug.Log(ex.Message);
            }

        }
        //Matoda koja salje POST zatev serveru
        private void POSTRequestCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            busy = false;
            if (e == null || e.Error != null || e.Result == null)
                throw new Exception("POST: " + senderId + " error.");
            else if (e.Cancelled)
                throw new Exception("POST: " + senderId + " cancelled.");
            else
            {
                POSTcomplete = true;
                data = Encoding.Default.GetString(e.Result);
            }
        }

    }
    #endregion
    #region WWW
    public class WWWClass : MonoBehaviour
    {
        public object result;
        public bool completed = false;
        public void DownloadTexture(string url)
        {
            completed = false;
            StartCoroutine("TextureCorutine",url);
        }

        IEnumerator TextureCorutine(string url)
        {
            var www = new WWW(url);
            yield return www;
            completed = true;
            result = www.texture;
        }

        public void DownloadText(string url)
        {
            completed = false;
            StartCoroutine("TextCorutine", url);
        }

        IEnumerator TextCorutine(string url)
        {
            var www = new WWW(url);
            yield return www;
            completed = true;
            result = www.text;
        }

        public void DownloadBundle(string url)
        {
            completed = false;
            StartCoroutine("BundleCorutine", url);
        }

        IEnumerator BundleCorutine(string url)
        {
            var www = new WWW(url);
            yield return www;
            completed = true;
            result = www.assetBundle;
        }

        void UploadFile(string localFileName, string uploadURL)
        {
            StartCoroutine(UploadFileCo(localFileName, uploadURL));
        }

        IEnumerator UploadFileCo(string localFileName, string uploadURL)
        {
            WWW localFile = new WWW("file:///" + localFileName);
            yield return localFile;
            if (localFile.error == null)
                Debug.Log("Loaded file successfully");
            else
            {
                Debug.Log("Open file error: " + localFile.error);
                yield break; // stop the coroutine here
            }
            WWWForm postForm = new WWWForm();
            // version 1
            //postForm.AddBinaryData("theFile",localFile.bytes);
            // version 2
            postForm.AddBinaryData("theFile", localFile.bytes, localFileName, "text/plain");
            WWW upload = new WWW(uploadURL, postForm);
            yield return upload;
            if (upload.error == null)
                Debug.Log("upload done :" + upload.text);
            else
                Debug.Log("Error during upload: " + upload.error);
        }
    }
    #endregion
}
