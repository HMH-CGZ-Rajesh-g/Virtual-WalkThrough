using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using cdb.virtualwalkthrough;
public class FullScreenVideoController : MonoBehaviour
{

    public VideoPlayer currentVideoPlayer;
    public GameObject playButton, pauseButton, exitButton;
    public Slider videoSlider;
    bool sliderClicked = false;
    public GameObject loadingObject;
    private object currentObject;
    public void Awake()
    {


            VirtualwalkThrough.VideoSetup += Setpurpose;
        GlobalConstants.AppActions.Add(gameObject.name + "Pause", () => { });

        GlobalConstants.AppActions.Add(gameObject.name + "PlayButton", () => { });
        GlobalConstants.AppActions.Add(gameObject.name + "ExitFullScreen", () => { });

    }
    public void OnEnable()
    {
        Debug.Log(gameObject.name);

        transform.GetChild(0).GetComponent<Button>().onClick.AddListener(()
        => VirtualwalkThrough.FireAppAction(gameObject.name + "Pause"));

        playButton.GetComponent<Button>().onClick.AddListener(()
                        => VirtualwalkThrough.FireAppAction(gameObject.name + "PlayButton"));


        exitButton.GetComponent<Button>().onClick.AddListener(()
            => VirtualwalkThrough.FireAppAction(gameObject.name + "ExitFullScreen"));
    }

    private void Update()
    {
        if (currentVideoPlayer != null && currentVideoPlayer.isPlaying)
            videoSlider.value = (float)currentVideoPlayer.frame / (float)currentVideoPlayer.frameCount;

    }
    public void Setpurpose(object objectType, bool setfullscreen)
    {

        currentObject = objectType;
        transform.GetChild(0).gameObject.SetActive(true);
        switch (objectType)
        {
            case VideoPlayer videoPlayer:
                currentVideoPlayer = videoPlayer;

                if (!currentVideoPlayer.isPlaying)
                {
                    playButton.SetActive(true);
                    videoSlider.gameObject.SetActive(false);
                }
                else
                {
                    playButton.SetActive(false);
                    videoSlider.gameObject.SetActive(true);
                }


                GlobalConstants.AppActions[gameObject.name + "Pause"] =
                () =>
                {
                    Debug.Log("Pausing video");
                    currentVideoPlayer.Pause();
                    playButton.SetActive(true);
                };



                GlobalConstants.AppActions[gameObject.name + "PlayButton"] =
                () =>
                {
                    PlayButton();

                };



                break;

            //case Texture2D texture2D:
            //    closeButton.SetActive(true);
            //    playButton.SetActive(false);



            //    GlobalConstants.AppActions[gameObject.name + "CloseButton"] =
            //    () =>
            //    {
            //        Debug.Log("Here");
            //        if (transform.parent.childCount > 1)
            //        {
            //            transform.parent.GetChild(1).gameObject.SetActive(true);

            //            transform.parent.GetChild(0).gameObject.SetActive(false);
            //        }
            //    };


            //    break;
        }

            currentVideoPlayer.renderMode = VideoRenderMode.RenderTexture;
            currentVideoPlayer.targetTexture = DataManager.instance.renderTexture;

            GlobalConstants.AppActions[gameObject.name + "ExitFullScreen"] =
            () =>
            {
                Debug.Log("Calling Here");

                ExitFullScreen();

            };

        if (setfullscreen)
            ExitFullScreen();
    }

    private void PlayButton()
    {
        if (currentVideoPlayer.isPrepared)
        {
            Debug.Log("Here");
            loadingObject.SetActive(false);
            currentVideoPlayer.Play();

            playButton.SetActive(false);
            videoSlider.gameObject.SetActive(true);

        }
        else
        {
            Debug.Log("Here 1");
            //currentVideoPlayer.renderMode = VideoRenderMode.MaterialOverride;
            //currentVideoPlayer.renderMode = VideoRenderMode.RenderTexture;
            playButton.SetActive(false);
            //currentVideoPlayer.Play();
            currentVideoPlayer.Prepare();

            loadingObject.SetActive(true);
            currentVideoPlayer.prepareCompleted += CurrentVideoPlayer_prepareCompleted;
        }
    }

    private void CurrentVideoPlayer_prepareCompleted(VideoPlayer source)
    {
        currentVideoPlayer.prepareCompleted -= CurrentVideoPlayer_prepareCompleted;

        PlayButton();
    }

    private void OnDisable()
    {
        playButton.GetComponent<Button>().onClick.RemoveAllListeners();
        pauseButton.GetComponent<Button>().onClick.RemoveAllListeners();


    }

    public void MouseDown()
    {
        currentVideoPlayer.Pause();

        sliderClicked = true;

    }
    public void MouseUp()
    {

        SlideValueChange();
        sliderClicked = false;

    }
    public void SlideValueChange()
    {
        float frames = (float)currentVideoPlayer.frameCount;
        currentVideoPlayer.frame = (long)(frames * videoSlider.value);
        currentVideoPlayer.Play();



    }

    
    public void ExitFullScreen()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        currentVideoPlayer.renderMode = VideoRenderMode.MaterialOverride;
        Debug.Log("ExitFullScreen");
        //currentVideoPlayer.targetMaterialRenderer = this.GetComponent<MeshRenderer>();
    }

    private void OnDestroy()
    {

            VirtualwalkThrough.VideoSetup -= Setpurpose;
    }
}
