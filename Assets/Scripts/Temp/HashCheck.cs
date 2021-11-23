using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HashCheck : MonoBehaviour
{
    public Image Test;
    AWSSignatureV4 signer;
    // Start is called before the first frame update
    void Start()
    {
        signer = new AWSSignatureV4();
        StartCoroutine(SampleRequest());
    }

    void Sign(string access, string secret,string session)
    {
        
    }

    IEnumerator SampleRequest()
    {
        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture("https://insurancenext-client-demos.s3.amazonaws.com/Images/Interactive-Home-Quote.jpg");
        var t = DateTimeOffset.UtcNow;
        webRequest.SetRequestHeader("Authorization", signer.CreateSignature("insurancenext-client-demos.s3.amazonaws.com", "/Images/Interactive-Home-Quote.jpg", t, "IQoJb3JpZ2luX2VjEJD//////////wEaCXVzLWVhc3QtMSJHMEUCIBffTXuIvvs8xwBtWVFV2YhZ87O7JO2qY4nLJQ6jfO8rAiEAjQkVi8vgAwK1UPLe24iM0Cf487CQpIe7JQAgjzFz6tgqzQQIqf//////////ARAEGgwzODk3ODI5MzAxMzUiDI9b52mRBNQnvptL6yqhBASiDwjUDeptr7YFq/clTzlsT/GpsrG9KPbzUzLxzE1h4UyMPm+1KEiS/GgoU7WBj4v1U0EFdBZdrooB1cd925rYmESLBPAwhkwPzBK9Nogprr/Aam/uuGz7Y5zhQOy2h9HqoBrPa0RUo8CUzad8d9mEm0OjrUMOKsid0Uv+fmfu+IEqxOgUrLkVcZnDK4Ac2XKyCktpP07f5MXD/Ww3IlOuNNIPhF0EB/DM2KxqYme6cjaoYUWyLm1WeG99XnqX5O29lp2tNHjwdjr0AzC0MX9vnRBD99yXrWtxmsuF22TnudCJxMUv4o7tedk9lXFKW2eNPSgmmxAQOiA9lh9xU8Koklg86NgOFr4lZufmU0Yor4HFdy9lFaiq4jCn9h9W7RKDTQG6iALZYc3aucd7zfHFtm+aDWx9/gI3c78rt50A+sCeQ8Kog7DYBvyQ8OISjiKuaU6V4H8yfYp0sUJ+6VyXNxJZP2XOb6VOJnhC6BzHBl8u+TSa4Pyyp2BoebghYmmvXygikEqAstOUFea/w2SANM36594LR+SvzrhF2D0mrD4llCuq7n+ssAsg5WX0vhrvG5/N+yGOlB5wZMMBfBS4DP877pzdIuEShPD4sDSxiy5eOAvFR9WkC+ArHArK9f0b3fF1/hHxqMnUrI7ZKuK8snMKO61iV2wgj9LE5ugt1OMiAAgnBaX+eu885i5Y/9fqpGey8XBmPfe1UM2+Jq40MLTZrfsFOoUCxcmJXLjyF3UJRPgrKPatL4YWn3h19NUdMyWYuQICXzPr9TYLfiR3WTED+HLwgNad3FEk0bXUnsnySKWuc7aJXReSFJlG9JaPBOY0a2kuVbpRk0+R7DEKjFfQGkFBvMTjHS55wEQTpzHOncwQb3IyUA/i6U+brC9pnMTxCioe9UxsMnltpG0yGKGvIuAE1b+32rDElfZSzcQ3h3fMRZhonJdwZgyWG1Io981jXmsCpbdjeELcFFCwuVauQn1nyi4kYbwZcPW3TEzqnJl2yn/CLehqw3qipoyLk1lxfpCcOJYtDyWSJnLRVU9dbQZ78XGYGW2Q++AWpf5jaWQhs1BJLPZ7qUw7", "TZF+2mdxtVSwJSQFC+wvSshuXk7MmwG8TE4j9amq", "ASIAVVQG7QLL5WPM664X"));
        webRequest.SetRequestHeader("host", "insurancenext-client-demos.s3.amazonaws.com");
        webRequest.SetRequestHeader("X-Amz-Content-Sha256", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855");
        webRequest.SetRequestHeader("X-Amz-Date", t.ToString("yyyyMMddTHHmmssZ"));
        webRequest.SetRequestHeader("X-Amz-Security-Token", "IQoJb3JpZ2luX2VjEJD//////////wEaCXVzLWVhc3QtMSJHMEUCIBffTXuIvvs8xwBtWVFV2YhZ87O7JO2qY4nLJQ6jfO8rAiEAjQkVi8vgAwK1UPLe24iM0Cf487CQpIe7JQAgjzFz6tgqzQQIqf//////////ARAEGgwzODk3ODI5MzAxMzUiDI9b52mRBNQnvptL6yqhBASiDwjUDeptr7YFq/clTzlsT/GpsrG9KPbzUzLxzE1h4UyMPm+1KEiS/GgoU7WBj4v1U0EFdBZdrooB1cd925rYmESLBPAwhkwPzBK9Nogprr/Aam/uuGz7Y5zhQOy2h9HqoBrPa0RUo8CUzad8d9mEm0OjrUMOKsid0Uv+fmfu+IEqxOgUrLkVcZnDK4Ac2XKyCktpP07f5MXD/Ww3IlOuNNIPhF0EB/DM2KxqYme6cjaoYUWyLm1WeG99XnqX5O29lp2tNHjwdjr0AzC0MX9vnRBD99yXrWtxmsuF22TnudCJxMUv4o7tedk9lXFKW2eNPSgmmxAQOiA9lh9xU8Koklg86NgOFr4lZufmU0Yor4HFdy9lFaiq4jCn9h9W7RKDTQG6iALZYc3aucd7zfHFtm+aDWx9/gI3c78rt50A+sCeQ8Kog7DYBvyQ8OISjiKuaU6V4H8yfYp0sUJ+6VyXNxJZP2XOb6VOJnhC6BzHBl8u+TSa4Pyyp2BoebghYmmvXygikEqAstOUFea/w2SANM36594LR+SvzrhF2D0mrD4llCuq7n+ssAsg5WX0vhrvG5/N+yGOlB5wZMMBfBS4DP877pzdIuEShPD4sDSxiy5eOAvFR9WkC+ArHArK9f0b3fF1/hHxqMnUrI7ZKuK8snMKO61iV2wgj9LE5ugt1OMiAAgnBaX+eu885i5Y/9fqpGey8XBmPfe1UM2+Jq40MLTZrfsFOoUCxcmJXLjyF3UJRPgrKPatL4YWn3h19NUdMyWYuQICXzPr9TYLfiR3WTED+HLwgNad3FEk0bXUnsnySKWuc7aJXReSFJlG9JaPBOY0a2kuVbpRk0+R7DEKjFfQGkFBvMTjHS55wEQTpzHOncwQb3IyUA/i6U+brC9pnMTxCioe9UxsMnltpG0yGKGvIuAE1b+32rDElfZSzcQ3h3fMRZhonJdwZgyWG1Io981jXmsCpbdjeELcFFCwuVauQn1nyi4kYbwZcPW3TEzqnJl2yn/CLehqw3qipoyLk1lxfpCcOJYtDyWSJnLRVU9dbQZ78XGYGW2Q++AWpf5jaWQhs1BJLPZ7qUw7");
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log("Error in downloading " + webRequest.error);
        }
        else
        {
                Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                Test.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100);
        }
    }
}
