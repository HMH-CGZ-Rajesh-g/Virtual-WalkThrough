using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Video;

public class PresentationManager : MonoBehaviour
{
    PresentationController _controller;
    [SerializeField] RenderTexture gTVTexture;
    [SerializeField] GameObject Teleporter;
    [SerializeField] GameObject quad;
    [SerializeField] GameObject close;
    [SerializeField] GameObject pause;
    public VideoController vController;
    [DllImport("__Internal")]
    private static extern void OpenUrl(string url);

    void Start()
    {
        _controller = transform.GetChild(0).GetComponent<PresentationController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        Teleporter.SetActive(true);
        // vController.GetComponent<VideoPlayer>().loopPointReached += OnVideoEnded;
        _controller.raycastEnabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        //vController.GetComponent<VideoPlayer>().loopPointReached -= OnVideoEnded;
        Teleporter.SetActive(false);
        _controller.raycastEnabled = false;
        _controller.HideExperienceBooth();
    }

    public void PlayVideoOnScreen(string url)
    {

#if UNITY_EDITOR
        Application.OpenURL(url);
#else
        OpenUrl(url);
#endif

    }

    public void URLOpener(string url)
    {
#if UNITY_EDITOR
        Application.OpenURL(url);
#else
        OpenUrl(url);
#endif
    }

    private void OnVideoEnded(VideoPlayer source)
    {
        quad.SetActive(false);
        pause.SetActive(false);
        close.SetActive(false);
    }

    public void PauseVideo()
    {
        vController.GetComponent<VideoPlayer>().Pause();
    }

    public void PlayVideo()
    {
        vController.GetComponent<VideoPlayer>().Play();
    }

    public void MoveToNext()
    {
        vController.GetComponent<VideoPlayer>().Stop();
        quad.SetActive(false);
        close.SetActive(false);
        pause.SetActive(false);
    }

}
