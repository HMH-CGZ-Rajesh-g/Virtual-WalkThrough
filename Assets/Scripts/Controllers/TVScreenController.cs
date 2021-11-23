using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TVScreenController : MonoBehaviour
{
    public RoomController rc;
    public string url;
    public MediaType type;
    public bool isDisabled;
    TextMeshProUGUI title;
    TextMeshProUGUI details;
    Image icon;
    public Image mainIcon;
    public TextMeshProUGUI mainTitle;
    [SerializeField] GameObject disableMain;

    private void Start()
    {
        icon = transform.GetChild(1).GetComponent<Image>();
        title = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        details = transform.GetChild(5).GetComponent<TextMeshProUGUI>();
    }

    public void SetAttributes(SolutionObject obj, List<Sprite> icons)
    {
        try
        {
            // Set image thumbnail
            if (ExperienceManager.ImageDictionary.TryGetValue(obj.thumbnail, out Sprite sprite))
            {
                GetComponent<Image>().sprite = sprite;
            }
            else
            {
                // Set default sprite
                Debug.LogError("No sprite");
            }

            //Set media type
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
            }
            else
            {
                // defaults to URL
                type = MediaType.URL;
                icon.sprite = icons[0];
                Debug.Log("Sprite setup url");
            }

            // Set source url
            if (!string.IsNullOrEmpty(obj.mediaSource))
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
                isDisabled = true;
            }

            // Set disable / enable 
            isDisabled = !obj.isEnabled;
            if (!string.IsNullOrEmpty(obj.mediaTitle) && !string.IsNullOrEmpty(obj.description))
            {
                title.text = obj.mediaTitle;
                details.text = obj.description;
            }
            else
            {
                Debug.Log("Set default solution");
            }
            transform.GetChild(3).gameObject.SetActive(isDisabled);
            mainIcon.sprite = icon.sprite;
            mainTitle.text = title.text;
            disableMain.SetActive(isDisabled);
        }
        catch(Exception e)
        {
            Debug.LogError("Error at : " + gameObject.name + " " + e.Message);
        }
    }

    public void RequestedAction()
    {
        Debug.Log(url);
        if (type == MediaType.URL)
        {
            RequestToOpenURL();
        }
        else if (type == MediaType.VIDEO)
        {
            RequestToPlayVideo();
        }
    }

    public void RequestToPlayVideo()
    {
        Debug.Log(url);
        rc.PlayVideoOnScreen(url);
    }

    public void RequestToOpenURL()
    {
        rc.URLOpener(url);
    }
}
