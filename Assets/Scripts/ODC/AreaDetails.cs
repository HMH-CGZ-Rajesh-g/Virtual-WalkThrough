using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.Video;

public class AreaDetails : MonoBehaviour
{



    /// <summary>
    /// Assigning Data
    /// </summary>
    /// <param name="details"></param>
    void AssignDetails(Details details)
    {

        foreach(KeyValuePair<string,object> pair in details.details)
        {
            if (pair.Value == typeof(Sprite))
            {

                transform.Find(pair.Key).GetComponent<Image>().sprite = pair.Value as Sprite;
            }
            if (pair.Value == typeof(TextMeshProUGUI))
            {

                transform.Find(pair.Key).GetComponent<TextMeshProUGUI>().text = pair.Value as String;
            }
            if(pair.Value == typeof(VideoClip))
            {
                transform.Find(pair.Key).GetComponent<VideoPlayer>().clip = pair.Value as VideoClip;

            }
        }

    }
}


