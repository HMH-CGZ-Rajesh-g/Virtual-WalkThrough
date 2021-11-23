using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class CognitoHelper : MonoBehaviour
{
    public string userPoolId;
    public string clientId;
    public TMP_InputField userName;
    public TMP_InputField password;
    public string identityPoolId;
    public string region;
    public TextMeshProUGUI error;
    //private Credentials _cred;
    //private CognitoIdentityModel _token;
    public TMP_InputField bucket;
    public string bucketFeed="virtualwalkthrough";
    public string key;
    int count = 0;
    int dynamicCount = 0;
    private bool isGameEnabled = false;
    bool isFirstFeed = true;
    int feedCount = 0;
    DateTime current;
   // public static Credentials Cred;
    public Button signIn;
    // public ExperienceManager manager;

    public string AccessKeyId, SecretKey;
    public static CognitoHelper instance;

    [DllImport("__Internal")]
    private static extern string CognitoSignIn(string userPoolId, string clientId, string username, string password, string gameObject, string method);

    [DllImport("__Internal")]
    private static extern string GetCognitoIdentityCredentials(string region, string userPoolId, string idToken, string identityPoolId, string gameObject, string method);
    [DllImport("__Internal")]
    private static extern string CheckExpiration(string gameObject, string method);
    [DllImport("__Internal")]
    private static extern string PreSignLink(string accessKeyId, string secretAccessKey, string region, string bucket, string key, string gameObject, string method);
    [DllImport("__Internal")]
    private static extern string PreSignPut(string accessKeyId, string secretAccessKey, string region, string bucket, string feedbackLink,  string gameObject, string method);
    [DllImport("__Internal")]
    private static extern string GetAWSBucketElements(string accessKeyId, string secretAccessKey, string region, string bucket, string gameObject, string method);
    [DllImport("__Internal")]
    private static extern string GetAWSBucketObjects(string accessKeyId, string secretAccessKey, string region, string bucket, string key, string gameObject, string method);


    public void OnSignIn()
    {
        //_cred = new Credentials();
        //_token = new CognitoIdentityModel();
        //Debug.Log(userName.text);
        //Debug.Log(password.text);
        current = DateTime.Now;
#if UNITY_EDITOR
        //manager.enabled = true;
        //AccessKeyId = "ASIAVVQG7QLLZGWJSCZF";
        //SecretKey = "CC/zfnyF4XGa1JxQI0Dh3vovzPh8uocbHJ+/afKn";
        //_cred.SessionToken = "IQoJb3JpZ2luX2VjEEgaCXVzLWVhc3QtMSJGMEQCIEIG+sofh1LU5GNVCWZL5q//9673N+G72okkWPWKpjnEAiAevNZGEXcJ52P/CYb7+jaw6CXstO78hy0VG+UReA8uiSrEBAhwEAQaDDM4OTc4MjkzMDEzNSIMxdpsg3EIe8kAgIHlKqEEMyVFFPA5OeWwDiAxDfd7sNoa7d9iboKJBPlyy0fIw3PXksySpIOktdwRNaSqkcpDtEkf5pTZb5JQMudHMM/xOUJzxsfNKJOS0SZ4aeurlXTcvJFnw8L9nBP4doY7bxrAVFHVICLO8RuRGm/wWLbUukLVygvyCxTashJ96eUJBD6BH0nKgVixBzcuN/ctO71mSJS/kU5Ykb0bBRxLsPzO4aNBbC3Xp+w1etNPvM9ry84tg2U/6ecyTcFfdLl+nA+yozGap/ycNOPavoWKBDavs4MrSECb5y5j1MEAwjCTHlGIYqOhANN4pM3FWVfg+DQUtgN59st3019k0b+1P2/sy5Z6iUFeqKgQL9yTYI3qiCTVly+Qxl2FnyMQsr3ODONq/iYjSGo4fvI/lkOKNo9ocFgOxSr0e9vPRLsuRZZREV4/hhwxPdVEGwEZgh7mF0V9fdMn9BfZX9ANd+uexClA8NgV4g6R8y+PyxpmyL3qrxMleg+xp8bUMA559BoLTeNUEnMdg6uOKxqvGVOZdjFPfj92dTBJBChc0zTIXre5Zn7de4EbzVSZYqIBPCXP7IZLvu9hdTD30+P/P4TkVik785ZkM2K3+05NdYuzllHqjR2k/0+YoC0CCuX3PUcxSdRvnKQaHU/CRjHhbnK8QSTDj7qyqFPw9ORf8Vl1UQy2HYDIkQ+eKhBWlhow0mq+4LZTrbxF93b9MEkqSA5vJX1qfhQw+YbW+wU6hgLz8bIRqC3f7MV0fNDuVscZHL97U+s+f+FVupseOhJ35aLqIrYKvIVH0E9rrV1vmEeB6dtll5in043FEFQDZt44CIGaWOKPT4t8qdW76eCZ9caBWW2H7PGiOTRqeegfBvvv/rXL921SfJZ8V4x41X0XE5KKnZBgsPEUTpCarQOMLBh7j6X0YXMi+v86UDOG6rJdVlhmREKCBHl3BU0vPe15q+GO221byarrtK19EEIbaB2S7FVDM5LWj0wWeNmhfKuXnm9wTplKKi/H26a/jxA14raS4VAi/nECqTIUtq5nMi2DfJ74gSmF9hJx2XIzFi5H1lrtKc3g6jjSxs2gqFm+jZnfDdjo";
        //Cred = _cred;
        //transform.GetChild(0).gameObject.SetActive(false);
        //transform.GetChild(1).gameObject.SetActive(true);
        //transform.GetChild(2).gameObject.SetActive(true);
#else
        if (string.IsNullOrEmpty(userName.text))
        {
            error.text = "Please enter a valid username";
            }
        else if (string.IsNullOrEmpty(password.text))
        {
            error.text = "Please enter a valid password";
        }
        else if (string.IsNullOrEmpty(bucket.text))
        {
            error.text = "Please enter a valid S3 bucket";
        }
        else {
        signIn.interactable = false;
        error.text = "";
        CognitoSignIn(userPoolId, clientId, userName.text, password.text, gameObject.name, "GetToken");
        }
#endif
    }

    public float TimeElapsed()
    {
        return Time.time;
    }


    public void Awake()
    {
        if(instance==null)
         instance = this;

        VirtualwalkThrough.UpdateFeedBack += VirtualwalkThrough_UpdateFeedBack;
    }

    private void OnDestroy()
    {
        VirtualwalkThrough.UpdateFeedBack -= VirtualwalkThrough_UpdateFeedBack;

    }

    public void GetToken(string data)
    {
        //if (!string.IsNullOrEmpty(data))
        //{
        //    try
        //    {
        //        _token = JsonConvert.DeserializeObject<CognitoIdentityModel>(data);
        //        if (!string.IsNullOrEmpty(_token.idToken.jwtToken) && !string.IsNullOrEmpty(_token.accessToken.jwtToken))
        //        {
        //            Debug.Log("Auth token generated");
        //            error.color = Color.blue;
        //            error.text = "Login Successfull!";

        //            GetCognitoIdentityCredentials(region, userPoolId, _token.idToken.jwtToken, identityPoolId, gameObject.name, "OnLogin");

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        error.color = Color.red;
        //        ClearErrorLogs();
        //        error.text = "Unable to Login user. Please contact admin";
        //        Debug.LogError(e);
        //    }
        //}
        //else
        //{
        //    error.color = Color.red;
        //    error.text = "Unable to Login user. Please contact admin";
        //    Invoke("ClearErrorLogs", 2f);
        //    Debug.LogError("Empty data recieved");
        //}
    }

    public void OnLogin(string data)
    {
        //if (!string.IsNullOrEmpty(data))
        //{
        //    try
        //    {
        //        Credentials awsCred = JsonConvert.DeserializeObject<Credentials>(data);
        //        if (!string.IsNullOrEmpty(awsCred.AccessKeyId) && !string.IsNullOrEmpty(awsCred.SecretKey))
        //        {
        //            _cred = awsCred;
        //            Cred = awsCred;
        //            error.color = Color.blue;
        //            error.text = "Getting config from bucket";
        //            GetConfig();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        error.color = Color.red;
        //        ClearErrorLogs();
        //        error.text = "Unable to Login user. Please contact admin";
        //        Debug.LogError(e);
        //    }
        //}
        //else
        //{
        //    error.color = Color.red;
        //    error.text = "Unable to Login user. Please contact admin";
        //    Invoke("ClearErrorLogs", 2f);
        //    Debug.LogError("Empty data recieved");
        //}
    }

    private void ClearErrorLogs()
    {

        signIn.interactable = true;
        userName.text = "";
        password.text = "";
        bucket.text = "";
        error.text = "";
    }

    public void GetConfig()
    {
        PreSignLink(AccessKeyId, SecretKey, region, bucket.text, key, gameObject.name, "FetchData");
    }

    public void GetFeedback()
    {
        isGameEnabled = true;
        PreSignLink(AccessKeyId, SecretKey, region, bucket.text, "StreamingAssets/Feedback.json", gameObject.name, "FetchFeedback");
    }

    public void SetCount()
    {
        //count = manager.configuration.clients.Count < 4 ? GetComponent<ExperienceManager>().configuration.clients.Count : 4;
        //dynamicCount = count;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && isGameEnabled)
        {
            Debug.Log("Getting Feedback from .....  " + bucketFeed);
            Debug.Log("Total time diff :: " + (DateTime.Now - current).TotalMinutes + " mins");
            if ((DateTime.Now - current).TotalMinutes > 45f)
            {
                CheckExpiration(gameObject.name, "TokenValidity"); ;
            }
            else
            {
                DynamicFeedback();
            }
        }

        if (Input.GetKeyDown(KeyCode.V) && isGameEnabled)
        {
            Debug.Log("Getting other Feedbacks.....");
            if (feedCount < count && (DateTime.Now - current).TotalMinutes > 45f)
            {
                isFirstFeed = false;
                CheckExpiration(gameObject.name, "TokenValidity");
            }
            else if (feedCount < count)
            {
                StartCoroutine(SubsequesntGetRequest());
            }
            else
            {
                Debug.Log("Feedback count is full :: " + feedCount);
            }
        }
    }

    public void TokenValidity(string data)
    {
        //Debug.Log("Token Validaity :: " + data);
        //if (!string.IsNullOrEmpty(data))
        //{
        //    try
        //    {
        //        Credentials awsCred = JsonConvert.DeserializeObject<Credentials>(data);
        //        if (!string.IsNullOrEmpty(awsCred.AccessKeyId) && !string.IsNullOrEmpty(awsCred.SecretKey))
        //        {
        //            current = DateTime.Now;
        //            AccessKeyId = awsCred.AccessKeyId;
        //            SecretKey = awsCred.SecretKey;
        //            _cred.SessionToken = awsCred.SessionToken;
        //            Cred = _cred;
        //            if (isFirstFeed) DynamicFeedback();
        //            else StartCoroutine(SubsequesntGetRequest()); ;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        error.color = Color.red;
        //        ClearErrorLogs();
        //        error.text = "Unable to Login user. Please contact admin";
        //        Debug.LogError(e);
        //    }
        //}
        //else
        //{
        //    error.color = Color.red;
        //    error.text = "Unable to Login user. Please contact admin";
        //    Invoke("ClearErrorlogs", 2f);
        //    Debug.Log("No expiration session data recieved!");
        //}
    }

   public void DynamicFeedback()
    {
        Debug.Log("Coroutine ::  DynamicFeedback");
        Debug.Log("Feedbacks required :: " + count);
        PreSignPut(AccessKeyId, SecretKey, region, bucketFeed, "https://" + bucketFeed + ".s3.amazonaws.com/Feedback.json", gameObject.name, "PutLinksGenerated");
    }

    private string jsonData = "";


    private void VirtualwalkThrough_UpdateFeedBack(List<Feedback> feedbacks)
    {
         jsonData = JsonConvert.SerializeObject(feedbacks);

        DynamicFeedback();

        //StartCoroutine(SendFeedBack(jsonStringTrial));

    }

    public void PutLinksGenerated(string data)
    {
        Debug.Log("Here");
        StartCoroutine(SendFeedback(data));
    }
    IEnumerator SendFeedback(string url)
    {
        //Feedback feedback = new Feedback();
        string json = jsonData;// "Test"; //JsonConverter(feedback);
        using (UnityWebRequest www = UnityWebRequest.Put(url, json))
        {


            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Upload complete!");
            }
        }
    }


    public void GetFeedBacks()
    {
        StartCoroutine(SubsequesntGetRequest());

    }

    IEnumerator SubsequesntGetRequest()
    {
        Debug.Log("Coroutine ::  SubsequesntGetRequest");
        yield return new WaitForSecondsRealtime(2f);
        GetAWSBucketElements(AccessKeyId, SecretKey, region, bucketFeed+"Feedbacks", gameObject.name, "GetDynamicFeedback");
    }

    public void GetDynamicFeedback(string json)
    {
        Debug.Log("json");

        //if (string.IsNullOrEmpty(json))
        //{
        //    Debug.LogError("Empty string in dynamic feedback");
        //}
        //else
        //{
        //    Debug.Log("Dynamic feedback from get :: " + json);
        //    try
        //    {
        //        List<string> feedBacks = manager.loadJson.GetJSONObject<List<string>>(json);
        //        if (feedBacks.Count > dynamicCount)
        //        {
        //            feedBacks.RemoveRange(dynamicCount, (feedBacks.Count - dynamicCount));
        //        }
        //        Debug.Log("feedback count :: " + feedBacks.Count);
        //        feedCount += feedBacks.Count;
        //        dynamicCount -= feedBacks.Count;
        //        if (feedBacks.Count <= count)
        //        {
        //            for (int i = 0; i < feedBacks.Count; i++)
        //            {
        //                GetAWSBucketObjects(AccessKeyId, SecretKey, region, bucketFeed, feedBacks[i], gameObject.name, "GetFeedbackObject");
        //            }
        //        }
        //    }

        //    catch (Exception e)
        //    {
        //        Debug.LogError("Could not parse dynamic feedback :: " + e.Message);
        //    }
        //}
    }

    public void GetFeedbackObject(string json)
    {
        Debug.Log("Got feedback object"+ json);
        //Feedback feed = manager.loadJson.GetJSONObject<Feedback>(json);
        //manager.feedManager.HandleDynamicFeedback(feed);
    }
}
