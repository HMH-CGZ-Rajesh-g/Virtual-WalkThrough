using com.cdbi.helper;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceManager : MonoBehaviour
{
    public Configuration configuration;
    List<Feedback> feedBacks;
    public CustomerFeedbackManager feedManager;
    [SerializeField] List<IconController> mPASolutions;
    [SerializeField] List<IconController> mindspace;
    [SerializeField] List<IconController> prototype;
    [SerializeField] List<TVScreenController> cafeSolutions;
    [SerializeField] List<TVScreenController> aGSolutions;
    [SerializeField] List<TVScreenController> aOSolutions;
    [SerializeField] List<TVScreenController> cxoSolutions;
    [SerializeField] List<TVScreenController> wareSolutions;
    [SerializeField] List<TVScreenController> lRSolutions;
    [SerializeField] TextMeshProUGUI guests;
    [SerializeField] Image companyLogo;
    [SerializeField] TextMeshProUGUI companyName;
    [SerializeField] List<Sprite> iconsSet;
    public static Dictionary<string, Sprite> ImageDictionary;
    private WebHelper helper;
    public LoadJSON loadJson;
    // TODO Solution set for each room
    // TODO refrence to each solution configurator for main projection area
    // TODO update text and sprite and mark solution as disable or enabled

    void OnEnable()
    {
        helper = new WebHelper();
        loadJson = new LoadJSON();
#if UNITY_EDITOR
        configuration = loadJson.GetJSONObject<Configuration>("configuration.json");
        feedBacks = loadJson.GetJSONObject<List<Feedback>>("feedbackForWall.json");
        feedManager.Initiate(feedBacks);
        for (int i = 0; i < configuration.clients.Count; i++)
        {
            if (i > 0)
            {
                guests.text += ", ";
            }
            guests.text += configuration.clients[i];
        }
        GetComponent<CognitoHelper>().SetCount();
        GetImagesURL();

#endif
    }

    public void FetchData(string url)
    {
        StartCoroutine(helper.FetchData(url, GetData));
    }

    public void FetchFeedback(string url)
    {
        StartCoroutine(helper.FetchData(url, GetFeedback));
    }

    /// <summary>
    /// Callback when the configuration.json is downloaded from S3
    /// </summary>
    /// <param name="json"></param>
    public void GetData(string json)
    {
        Debug.Log("Got data from server");
        GetComponent<CognitoHelper>().GetFeedback();
        configuration = loadJson.GetJSONObject<Configuration>(json);
        if (!string.IsNullOrEmpty(json))
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            CognitoHelper cognito = GetComponent<CognitoHelper>();
            cognito.error.color = Color.red;
            cognito.error.text = "Unable to configure the app, bucket name could be wrong.";
            cognito.Invoke("ClearErrorLogs", 2f);
        }
        Debug.Log(configuration.cafe[0].mediaSource);

        for (int i = 0; i < configuration.clients.Count; i++)
        {
            if (i > 0)
            {
                guests.text += ", ";
            }
            guests.text += configuration.clients[i];
        }

        GetImagesURL();
    }

    public void GetFeedback(string json)
    {
        GetComponent<CognitoHelper>().SetCount();
        feedBacks = loadJson.GetJSONObject<List<Feedback>>(json);
        feedManager.Initiate(feedBacks);
    }

    /// <summary>
    /// Get Image URLS from configuration object for each room 
    /// </summary>
    void GetImagesURL()
    {
        List<string> imagesURL = new List<string>();

        if (!string.IsNullOrEmpty(configuration.companyLogo))
        {
            imagesURL.Add(configuration.companyLogo);
            companyName.gameObject.SetActive(false);
        }

        for (int i = 0; i < configuration.cafe.Count; i++)
        {
            imagesURL.Add(configuration.cafe[i].thumbnail);
        }
        for (int i = 0; i < configuration.autoGarage.Count; i++)
        {
            imagesURL.Add(configuration.autoGarage[i].thumbnail);
        }
        for (int i = 0; i < configuration.agentsOffice.Count; i++)
        {
            imagesURL.Add(configuration.agentsOffice[i].thumbnail);
        }
        for (int i = 0; i < configuration.cxoCabin.Count; i++)
        {
            imagesURL.Add(configuration.cxoCabin[i].thumbnail);
        }
        for (int i = 0; i < configuration.warehouse.Count; i++)
        {
            imagesURL.Add(configuration.warehouse[i].thumbnail);
        }
        for (int i = 0; i < configuration.livingRoom.Count; i++)
        {
            imagesURL.Add(configuration.livingRoom[i].thumbnail);
        }
        for (int i = 0; i < configuration.prototypeLab.Count; i++)
        {
            if (configuration.prototypeLab[i].mediaType == "image")
            {
                imagesURL.Add(configuration.prototypeLab[i].mediaSource);
            }
        }
        for (int i = 0; i < configuration.mindSpace.Count; i++)
        {
            if (configuration.mindSpace[i].mediaType == "image")
            {
                imagesURL.Add(configuration.mindSpace[i].mediaSource);
            }
        }

        Debug.Log("List of images generated");
        StartCoroutine(helper.GetSpriteDictionary(imagesURL, SetThumbnails, GetComponent<Loader>().UpdateProgress));
    }

    /// <summary>
    /// Set company logo and name
    /// </summary>
    /// <param name="obj"></param>
    public void SetThumbnails(Dictionary<string, Sprite> obj)
    {
        transform.GetChild(2).gameObject.SetActive(false);
        GetComponent<UIManager>().enabled = true;
        ImageDictionary = obj;

        // Set company logo
        if (ImageDictionary.TryGetValue(configuration.companyLogo, out Sprite sprite))
        {
            companyLogo.sprite = sprite;
            companyLogo.SetNativeSize();
        }

        // If company logo is not set in configuration json disable object and check for company name
        else
        {
            companyLogo.gameObject.SetActive(false);
            if (!string.IsNullOrEmpty(configuration.company))
            {
                companyName.text = configuration.company;
            }
            else
            {
                companyName.gameObject.SetActive(false);
            }
        }
        Debug.Log("Setting solutions");
        // Check if all data is available
        SolutionSetter();
    }

    /// <summary>
    /// Set attributes for each solution in room
    /// </summary>
    void SolutionSetter()
    {
        GetComponent<CognitoHelper>().bucketFeed = configuration.feedbackBucket;
        for (int i = 0; i < configuration.mainProjectionArea.Count; i++)
        {
            mPASolutions[i].SetAttributes(configuration.mainProjectionArea[i], iconsSet);
        }
        Debug.Log(configuration.mindSpace[0].mediaSource);
        for (int i = 0; i < configuration.mindSpace.Count; i++)
        {
            mindspace[i].SetAttributes(configuration.mindSpace[i], iconsSet);
        }
        for (int i = 0; i < configuration.prototypeLab.Count; i++)
        {
            prototype[i].SetAttributes(configuration.prototypeLab[i], iconsSet);
        }
        for (int i = 0; i < configuration.cafe.Count; i++)
        {
            cafeSolutions[i].SetAttributes(configuration.cafe[i], iconsSet);
        }
        for (int i = 0; i < configuration.autoGarage.Count; i++)
        {
            aGSolutions[i].SetAttributes(configuration.autoGarage[i], iconsSet);
        }
        for (int i = 0; i < configuration.agentsOffice.Count; i++)
        {
            aOSolutions[i].SetAttributes(configuration.agentsOffice[i], iconsSet);
        }
        for (int i = 0; i < configuration.cxoCabin.Count; i++)
        {
            cxoSolutions[i].SetAttributes(configuration.cxoCabin[i], iconsSet);
        }
        for (int i = 0; i < configuration.warehouse.Count; i++)
        {
            wareSolutions[i].SetAttributes(configuration.warehouse[i], iconsSet);
        }
        for (int i = 0; i < configuration.livingRoom.Count; i++)
        {
            lRSolutions[i].SetAttributes(configuration.livingRoom[i], iconsSet);
        }
    }
}
