
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;
using Newtonsoft.Json;

public class LoadJsonFile : MonoBehaviour
{
   public void LoadWithCustomerID(string ID, ODCRoot data, Action<bool,ODCRoot> _action)
    {



        Payload payload = new Payload();
        payload.customerID = ID;// "626a4";

        string loadfile = JsonUtility.ToJson(payload);

        StartCoroutine(CallLogin("https://dev-api.toybox.dev/virtual-walkthrough/fetch", loadfile,data,_action));
    }

    public IEnumerator CallLogin(string url, string logindataJsonString, ODCRoot data, Action<bool,ODCRoot> action =null)
    {

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(logindataJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Erro: " + request.error);
            action.Invoke(false,null);
        }
        else
        {
            Debug.Log("All OK");
            Debug.Log(request.downloadHandler.text);


           data = JsonConvert.DeserializeObject<ODCRoot>(request.downloadHandler.text);

            if(data.Items.Count==1)
            action.Invoke(true,data);
            else
                action.Invoke(false, null);



        }

    }

    //IEnumerator Upload(string _object)
    //{


    //    UnityWebRequest www = UnityWebRequest.Post("https://dev-api.toybox.dev/virtual-walkthrough/fetch", _object);
    //    yield return www.SendWebRequest();

    //    if (www.error == null)
    //    {
    //        Debug.Log("Form upload complete!"+ www.downloadHandler.text);

    //    }
    //    else
    //    {
    //        Debug.Log(www.error);

    //    }
    //}
}



// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
[Serializable]
public class Payload
{
    public string customerID;
}
