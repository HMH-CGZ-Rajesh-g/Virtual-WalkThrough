using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using cdb.virtualwalkthrough;
using UnityEngine.Networking;

public class VirtualwalkThrough : MonoBehaviour
{
    public static event Action<string> IDEnterd;

    public static event Action<List<SpawanObject>> SpawnObjects;

    public static event Action<VideoPlayer> PlayVideo;
    public static event Action<StagingArea> UIManagerInput;
    public static event Action<Dictionary<string,Action>> AreaActions;
    public static event Action<string> AppAction;

    public static event Action<string> SendMessageToClient;
    public static event Action<List<Feedback>> LoadFeedback, UpdateFeedBack;
     public static event Action<Feedback>  Feed_feedBack ;

    public static event Action<KeyCode> GameKeys;
    public static event Action<string> Collisison_Trigger;
    public static event Action<AgendaDetails> CurrentAgendaDetails;
    public static event Action<object,bool> VideoSetup;
    public static event Action<int> NumberofAssetsToLoad;
    public static event Action AssetDownloaded;

    public static Action<Camera> AsignvideoCamera;

    public static void FireNumberofAssetsToLoad(int number )
    {
        NumberofAssetsToLoad?.Invoke(number);
    }

    public static void FireAssetDownloaded()
    {
        AssetDownloaded?.Invoke();
    }

    public static void FireInstantiateObject(List<SpawanObject> objects)
    {
        SpawnObjects?.Invoke(objects);
    }

    internal static void FireID(string currentID)
    {
        IDEnterd?.Invoke(currentID);
    }

    public static void FirePlayVideo(VideoPlayer videoPlayer)
    {
        PlayVideo?.Invoke(videoPlayer);
    }

    public static void FireUIManagerInput(StagingArea stagingArea)
    {
        UIManagerInput?.Invoke(stagingArea);
    }

    internal static void EnableObject(GameObject obj,bool enable)
    {
        obj.SetActive(enable);
    }

    public static void FireAreaActions(Dictionary<string, Action> keyValuePairs)
    {
        AreaActions?.Invoke(keyValuePairs);
    }

    public static void FireVideoSetup(object obj, bool setfullscreen)
    {
        VideoSetup?.Invoke(obj ,setfullscreen);
    }

    internal static void FireAssignvideoCamera(Camera _camera)
    {
        AsignvideoCamera?.Invoke(_camera);
    }
    public static void FireAppAction(string str)
    {
        if(GlobalConstants.AppActions.ContainsKey(str))
            {
            GlobalConstants.AppActions[str].Invoke();
        }

        if (GlobalConstants.isAdmin)
        {
            SendMessageToClient?.Invoke(str);
        }

    }

    public static void FireLoadFeedback(List<Feedback> feedbacks)
    {
        LoadFeedback?.Invoke(feedbacks);
    }
    public static void FireUpdateFeedBack(List<Feedback> feedbacks)
    {
        
        UpdateFeedBack?.Invoke(feedbacks);
    }


    public static void FireKeyActon(KeyCode keyCode)
    {
        GameKeys?.Invoke(keyCode);
    }

    public static void FireFeed_FeedBack(Feedback feedback)
    {
        Feed_feedBack?.Invoke(feedback);
    }

    public static void FireCollisisonTrigger(string str)
    {
        Collisison_Trigger?.Invoke(str);
    }

    public static void FireCurrentAgendaDetails(AgendaDetails agendaDetails)
    {
        CurrentAgendaDetails?.Invoke(agendaDetails);
    }
}

public struct SpawanObject
{
    public GameObject gameObject;
    public Vector3 pos;
    public Vector3 rotation;

    public SpawanObject(GameObject gameObject) : this()
    {
        this.gameObject = gameObject;
    }



}


