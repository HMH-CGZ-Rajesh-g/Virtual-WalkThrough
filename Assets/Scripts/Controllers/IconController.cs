using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconController : MonoBehaviour
{
    public LabController controller;
    public string url;
    Sprite _sprite;
    MediaType type;
    TextMeshProUGUI title;
    Image icon;

    private void Start()
    {
        icon = transform.GetChild(1).GetComponent<Image>();
        title = transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void SetAttributes(MainProjectionArea obj, List<Sprite> icons)
    {
        try
        {
            if (!string.IsNullOrEmpty(obj.mediaType) && Enum.TryParse(obj.mediaType.ToUpper(), out MediaType typ))
            {
                type = typ;
                if (typ == MediaType.URL)
                {
                    icon.sprite = icons[0];
                }
                else if (typ == MediaType.VIDEO)
                {
                    icon.sprite = icons[1];
                }
                else
                {
                    icon.sprite = icons[2];
                }
            }

            // Set source url
            if (!string.IsNullOrEmpty(obj.mediaSource))
            {
                if (type != MediaType.IMAGE)
                {
                    url = obj.mediaSource;
                    if (Regex.IsMatch(url, "^(http|https)://", RegexOptions.IgnoreCase))
                    {
                        Debug.Log(url);
                    }
                    else
                    {
                        url = "StreamingAssets/" + url;
                    }
                }
                else
                {
                    if (ExperienceManager.ImageDictionary.TryGetValue(obj.mediaSource, out Sprite sprite))
                    {
                        _sprite = sprite;
                    }
                    else
                    {
                        // Set default sprite
                        Debug.LogError("No sprite");
                    }
                }
            }
            else
            {
                Debug.Log("No url");
                // Default url
            }
            

            if (!string.IsNullOrEmpty(obj.mediaTitle))
            {
                title.text = obj.mediaTitle;
            }
            else
            {
                Debug.Log("Set default solution");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error at : " + gameObject.name + " " + e.Message);
        }
    }


    public void RequestedAction()
    {
        if (type == MediaType.URL)
        {
            RequestToOpenURL();
        }
        else if (type == MediaType.VIDEO)
        {
            RequestToPlayVideo();
        }
        else if (type == MediaType.IMAGE)
        {
            controller.DisableIcons();
            controller.DisplayPicture(_sprite);
        }

    }

    public void RequestToPlayVideo()
    {
        Debug.Log(url);
        controller.PlayVideoOnScreen(url);
    }

    public void RequestToOpenURL()
    {
        controller.URLOpener(url);
    }
}

public enum MediaType
{
    VIDEO,
    URL,
    IMAGE
}
