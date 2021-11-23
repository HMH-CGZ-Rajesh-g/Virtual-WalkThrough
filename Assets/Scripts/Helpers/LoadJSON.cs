using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public class LoadJSON
{
    public T GetJSONObject<T>(string path)
    {
        try
        {
            string file = Path.Combine(Application.streamingAssetsPath + "/", path);
#if UNITY_EDITOR
            string json = File.ReadAllText(file);
#else
        string json = path;
#endif
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch (Exception e)
        {
            Debug.Log("Error in converting configuration JSON :: " + e.Message);
            return default(T);
        }
    }
}
