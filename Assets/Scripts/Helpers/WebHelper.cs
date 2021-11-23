using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
/*
Author : BhomitB, 18th July2020
Revsion: 
Standard Networking class to cover web requests
*/
namespace com.cdbi.helper
{
    public class WebHelper
    {
        /// <summary>
        /// Fetch text/json data and return via callback method
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator FetchData(string url, Action<string> callback)
        {
            string path = "";
            if (url.Contains("http://") || url.Contains("https://"))
            {
                path = url;
            }
            else
            {
                path = "/StreamingAssets/" + url;
            }
#if UNITY_EDITOR
            path = "localhost:80" + path;
#endif
            if (!string.IsNullOrEmpty(path))
            {
                Debug.Log(path);
                using (UnityWebRequest www = UnityWebRequest.Get(path))
                {
                    yield return www.SendWebRequest();
                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.LogError("Error fetch data from : " + url + "got response : " + www.error);
                        callback(null);
                    }
                    else callback(www.downloadHandler.text);
                }
            }
            else
            {
                Debug.LogError("Please provide a valid url");
            }
        }

        public IEnumerator DownlodImage(string url, Action<Texture2D> callback)
        {
            if (!string.IsNullOrEmpty(url))
            {
                using (UnityWebRequest w = UnityWebRequestTexture.GetTexture(url))
                {
                    w.SetRequestHeader("Access-Control-Allow-Origin", "*");
                    w.SetRequestHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
                    yield return w.SendWebRequest();
                    if (w.isNetworkError || w.isHttpError)
                    {
                        Debug.LogError(w.error);
                        callback(null);
                    }
                    else
                    {
                        // Get the texture from download
                        Texture2D texture = DownloadHandlerTexture.GetContent(w);
                        callback(texture);
                    }
                }
            }
            else
            {
                Debug.LogError("Please provide a valid url");
            }
        }

        public IEnumerator GetSpriteDictionary(List<string> urls, Action<Dictionary<string, Sprite>> thumbnailsDict, Action<float> Loading)
        {
            SpriteUtility util = new SpriteUtility();
            Dictionary<string, Sprite> images = new Dictionary<string, Sprite>();
            string path = "";
            AWSSignatureV4 signer = new AWSSignatureV4();
            for (int i = 0; i < urls.Count; i++)
            {
                if (!images.ContainsKey(urls[i]))
                {
                    

                    if(urls[i].Contains("https://") || urls[i].Contains("https://"))
                    {
                        path = urls[i];
                    }
                    else
                    {
                        path = "/StreamingAssets/" + urls[i];
                    }
#if UNITY_EDITOR
                   // path = "localhost:80" + path;
#endif
                    UnityWebRequest w = new UnityWebRequest(path);
                    w.SetRequestHeader("Access-Control-Allow-Origin", "*");
                    w.SetRequestHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
                    if (urls[i].Contains(".s3.amazonaws.com"))
                    {
                        Uri uri = new Uri(urls[i]);
                        DateTimeOffset t = DateTimeOffset.UtcNow;
                        //w.SetRequestHeader("Authorization", signer.CreateSignature(uri.Host, uri.LocalPath, t, CognitoHelper.Cred.SessionToken, CognitoHelper.Cred.SecretKey, CognitoHelper.Cred.AccessKeyId));
                        w.SetRequestHeader("host", uri.Host);
                        w.SetRequestHeader("X-Amz-Content-Sha256", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855");
                        w.SetRequestHeader("X-Amz-Date", t.ToString("yyyyMMddTHHmmssZ"));
                      //  w.SetRequestHeader("X-Amz-Security-Token", CognitoHelper.Cred.SessionToken);
                    }
                    DownloadHandlerTexture texture = new DownloadHandlerTexture(true);
                    w.downloadHandler = texture;
                    yield return w.SendWebRequest();
                    if (w.isNetworkError || w.isHttpError)
                    {
                        Debug.Log("Error in downloading " + urls[i] + " " + w.error);
                    }
                    else
                    {
                        if (w.downloadHandler.isDone)
                        {
                            Loading(float.Parse((i+1).ToString())/float.Parse(urls.Count.ToString()));
                            Sprite image = util.textureToSprite(texture.texture);
                            images.Add(urls[i], image);
                        }
                    }
                }

            }
            thumbnailsDict(images);
        }
    }
}


