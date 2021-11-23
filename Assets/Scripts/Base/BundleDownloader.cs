using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
/// <summary>
/// This class will download and store all bundles and streaming images
/// </summary>
public class BundleDownloader : MonoBehaviour
{
    //[SerializeField] ErrorPopUpHandler errorPopUpHandler;
    private Dictionary<string, AssetBundle> assetBundleDict = new Dictionary<string, AssetBundle>();
    private Dictionary<string, AssetBundle> avatarAssetBundleDict = new Dictionary<string, AssetBundle>();
    private Dictionary<string, Texture2D> streamingImages = new Dictionary<string, Texture2D>();
    private float retryDelay = 1; // in seconds
    private int retryAttempts = 2; //three tries i.e 0,1,2
    private const int TIMED_OUT = 100; //number of seconds to wait before aborting a download

    //removing SessionManager
    //private SessionManager manager;

    public static event Action AllBundleDownloadFinished;

    public static event Action AllStreamingImagesFinished;

    public static event Action<bool>   AllImagesFinished;

    public static event Action UnloadSpecificBundleFinished;

    public static event Action UnloadFinished;

    private void Awake()
    {
        //errorPopUpHandler = GameObject.FindGameObjectWithTag("ErrorCanvas").GetComponent<ErrorPopUpHandler>();
    }
    /// <summary>
    /// Downloads all bundles for in the bundle path provided
    /// </summary>
    /// <param name="bundle_paths"></param>
    /// <returns></returns>
    public IEnumerator DownloadAllBundles(List<string> bundle_paths)
    {
        Debug.Log("Starting download");
        Caching.ClearCache();

        string error = string.Empty;
        string bundleUrl = string.Empty;
        
        for (int i = 0; i < bundle_paths.Count; i++)
        {
          int tries = 0;
          bundleUrl = bundle_paths[i];// .ToLower ()//make sure the path in asset server path is lowercase

            string bundleName = GetBundleNameFromUrl (bundleUrl); //.ToLower()
          
            if (assetBundleDict.ContainsKey(bundleName) && assetBundleDict[bundleName] == null)
            {
                assetBundleDict.Remove(bundleName);
            }

            if (!assetBundleDict.ContainsKey(bundleName))
            {
                do
                {
                    ++tries;
                    using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl))
                    {
                        www.timeout = TIMED_OUT;

                        yield return www.SendWebRequest();

                        error = ErrorHandler(error, bundleUrl, bundleName, www);

                        if (string.IsNullOrEmpty(error) && www != null)
                        {
                            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                            if (www.downloadedBytes > 0 && bundle)
                            {
                                // Debug.Log ("Downloaded bundle: " + bundle);
                                assetBundleDict.Add(bundleName, bundle);
                                tries = 0;
                                error = string.Empty;
                                break;
                            }
                            else
                            {
                                error = "Unknown Error";

                                /// Unloading partially downloaded bundle if any
                                if (bundle != null)
                                {
                                    bundle.Unload(true);
                                }
                                string assetName = bundleName.Replace('_', ' ');
                               // errorPopUpHandler.ShowPopUp($"Check your internet connection and try to log in again.\n<color=red><size=16>Error ({assetName})</size></color>");
                                Debug.LogError(string.Format("Error: {0} - Bundle {1}, in {2}. Check for corrupted bundles", error, bundleName, bundleUrl));
                            }
                        }
                    }

                    if (tries <= retryAttempts)
                    {
                        Debug.Log(string.Format("Retrying to download {0}", bundleName));
                        yield return new WaitForSeconds(retryDelay);
                        error = string.Empty;
                    }
                }
                while (tries <= retryAttempts);

                // Throw error if the bundle is not downloaded
                if (tries > retryAttempts)
                {
                    Debug.LogError("tries " + tries);
                    string assetName = bundleName.Replace('_', ' ');
                    //errorPopUpHandler.ShowPopUp($"Check your internet connection and try to log in again.\n<color=red><size=16>Error ({assetName})</size></color>");
                }
            }
            VirtualwalkThrough.FireAssetDownloaded();
        }

        if (AllBundleDownloadFinished != null && string.IsNullOrEmpty(error))
        {
            AllBundleDownloadFinished();
        }

    }

    private static string ErrorHandler(string error, string bundleUrl, string bundleName, UnityWebRequest www)
    {
        if (!string.IsNullOrEmpty(www.error))
        {
            error = www.error;
            Debug.LogError(string.Format("Error: {0} - Bundle {1}, in {2}", error, bundleName, bundleUrl));

            /// Unloading partially downloaded bundle if any
            if (www.downloadedBytes > 0)
            {
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                if (bundle != null)
                {
                    bundle.Unload(true);
                }
            }
        }

        return error;
    }
    
    /// <summary>
    /// splits bundle path to get bundle name
    /// </summary>
    /// <param name="bundle_url"></param>
    /// <returns></returns>
    public string GetBundleNameFromUrl(string bundle_url)
    {
        string[] splitPath = bundle_url.Split('/');
        return splitPath[splitPath.Length - 1];
    }

    /// <summary>
    /// Returns loaded bundle by passing bundle name
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>

    public AssetBundle GetAssetBundle(string bundleName)
    {
        bundleName = bundleName.ToLower();
        //Debug.Log(bundleName);
        if (assetBundleDict.ContainsKey(bundleName))
        {
            return assetBundleDict[bundleName];
        }
        else
        {
            //if (manager.silentAudio && IsAudioFile(bundleName))
            //{
            //    //Debug.LogError(string.Format("Key not found exception! Replacing with silent audio. Bundle {0} is not downloaded and you are trying to access it", bundleName));
            //    return manager.silentAudio;
            //}

            //Debug.LogError(string.Format("Key not found exception! Returning null. Bundle {0} is not downloaded and you are trying to access it", bundleName));
            return null;
        }
    }

    public bool Contains(string bundleName)
    {
        if (assetBundleDict.ContainsKey(bundleName))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Dictionary<string, AssetBundle> GetAssetBundleDictionary()
    {
        return assetBundleDict;
    }

    /// <summary>
    /// Load all streaming audios in given list of paths
    /// </summary>
    /// <param name="filePaths"></param>
    /// <returns></returns>
    public IEnumerator LoadImagesFromFilePath(List<string> filePaths, bool isJpeg = false)
    {
        string errorMsg = string.Empty;
        string extension = ".png";
        if (isJpeg)
        {
            extension = ".jpg"; // Use AllImagesFinished event when trying to download pngs and jpegs in the same activity.
        }
        
        for (int i = 0; i < filePaths.Count; i++)
        {
            string fileName =  filePaths[i];
            Debug.Log("before" + fileName);

            string url = filePaths[i];// Utility.GetStreaminAssetsPath()+ filePaths[i] + extension;
            //Debug.Log("image url: " + url);

            if (!streamingImages.ContainsKey(fileName))
            {
                int tries = 0;
                UnityWebRequest wr = null;
                do
                {
                    tries++;
                    wr = new UnityWebRequest(url);
                    DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
                    wr.downloadHandler = texDl;


                    yield return wr.SendWebRequest();
                    if (!(wr.isNetworkError || wr.isHttpError))
                    {
                        tries = 0;//resetting tries on successful download
                        streamingImages.Add(fileName, texDl.texture);
                        Debug.Log("downloaded" + fileName);
                        break;//break the loop
                    }
                    else
                    {
                        errorMsg = wr.error;
//                        Debug.Log($"Retrying to download image {fileName}\t retryCount is {tries}");
                    }
                } while (tries <= retryAttempts);

                if (tries > retryAttempts)//Show errorPopUp
                {
                    Debug.Log("tried_many Times " + fileName );
                    //string assetName = fileName.Replace('_', ' ');
                    //errorPopUpHandler.ShowPopUp($"Check your internet connection and try to log in again.\n<color=red><size=16>Error ({assetName})</size></color>");
                }
            }

            VirtualwalkThrough.FireAssetDownloaded();

        }

        if (AllStreamingImagesFinished != null)
        {
            AllStreamingImagesFinished();
        }

        if (AllImagesFinished != null)
        {
            AllImagesFinished(isJpeg);
        }
    }

    /// <summary>
    /// Returns a dictionary of image lists.
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, Texture2D> GetStreamedImageDictionary()
    {
        return streamingImages;
    }

    /// <summary>
    /// Returns loaded bundle by passing bundle name
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public Sprite GetStreamingImage(string fileName)
    {
        //fileName = fileName.ToLower();
        //Debug.Log(fileName);
        Texture2D tex = null;
        if (streamingImages.ContainsKey(fileName))
        {
            tex = streamingImages[fileName];
        }
        else
        {
            return null;
        }

        return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    /// <summary>
    /// Returns only image
    /// </summary>
    /// <param name="texName"></param>
    /// <returns></returns>
    public Texture2D GetImage(string texName)
    {
        if (streamingImages.ContainsKey(texName))
            return streamingImages[texName];
        else
            return null;
    }





    /// <summary>
    /// Unloading all bundles
    /// </summary>
    public void OnDestroy()
    {
        UnloadAllBundles();
    }

    public void UnloadAllBundles()
    {
        foreach (KeyValuePair<string, AssetBundle> kvp in assetBundleDict)
        {
            if (kvp.Value != null)
            {
                kvp.Value.Unload(true);
                Caching.ClearAllCachedVersions(kvp.Key);
            }
        }

        foreach (KeyValuePair<string, AssetBundle> kvp in avatarAssetBundleDict)
        {
            if (kvp.Value != null)
            {
                kvp.Value.Unload(true);
                Caching.ClearAllCachedVersions(kvp.Key);
            }
        }

        assetBundleDict.Clear();
        streamingImages.Clear();
            
        if (UnloadFinished != null)
        {
            UnloadFinished();
        }
    }

    public void UnloadSpecificBundles(List<string> keys)
    {
        foreach (string key in keys)
        {
            if (assetBundleDict.ContainsKey(key))
            {
                if (assetBundleDict[key] != null)
                {
                    assetBundleDict[key].Unload(true);
                }

                assetBundleDict.Remove(key);
            }
        }

        if (UnloadSpecificBundleFinished != null)
        {
            UnloadSpecificBundleFinished();
        }
    }

  

    public void UnloadBundle(string bundleName)
    {
        assetBundleDict[bundleName].Unload(true);
        assetBundleDict.Remove(bundleName);
    }



}
