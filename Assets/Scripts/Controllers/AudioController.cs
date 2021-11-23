using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// Author: BhomitB, 13 Aug 2020
/// Audio Controller for WebGL only to stream MP3's using jslib (native code)
/// </summary>
public class AudioController : MonoBehaviour
{
    public string audioSource;
    [DllImport("__Internal")]
    private static extern void PlayAudio(string url, string gameobject, string method);
    [DllImport("__Internal")]
    private static extern void PauseAudio();

    [DllImport("__Internal")]
    private static extern void ResumeAudio();
    private bool isPlaying = false;
    private bool isFirst = false;
    [SerializeField] GameObject highlight;
    public void RequestPlayAudio()
    {
        if (!string.IsNullOrEmpty(audioSource))
        {
            PlayAudio(audioSource, gameObject.name, "DropHighlight");
        }
        else
        {
            Debug.LogError("Audio source is null or empty");
        }
    }

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            var selection = hitInfo.transform;
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (gameObject.name == hitInfo.transform.name && !isPlaying)
                {
                    highlight.SetActive(true);
                    isFirst = false;
                    isPlaying = true;
                    RequestPlayAudio();
                }
                //else if (gameObject.name == hitInfo.transform.name && !isPlaying && !isFirst)
                //{
                //    ResumeAudio();
                //    highlight.SetActive(true);
                //    isPlaying = true;
                //}
                //else if (gameObject.name == hitInfo.transform.name && isPlaying)
                //{
                //   PauseAudio();
                //    DropHighlight();
                //    isPlaying = false;
                //}
            }
        }
    }

    public void DropHighlight()
    {
        isPlaying = false;
        highlight.SetActive(false);
    }
}
