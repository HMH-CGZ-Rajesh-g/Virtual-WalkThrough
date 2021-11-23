using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using cdb.virtualwalkthrough;
public class VideoController1 : MonoBehaviour
{

    public VideoPlayer currentVideoPlayer;
    public GameObject playButton, closeButton, pauseButton, expandButton;
    public Slider videoSlider;
    bool sliderClicked = false;
    public GameObject loadingObject;
    private object currentObject;
    public void Awake()
    {
        //playButton = transform.GetChild(0).GetChild(0).gameObject;
        //closeButton = transform.GetChild(0).GetChild(1).gameObject;
        //pauseButton = transform.GetChild(0).gameObject;


        VirtualwalkThrough.AsignvideoCamera += AssignCamera;

        GlobalConstants.AppActions.Add(gameObject.name + "CloseButton", () => { Debug.Log("CloseButton"); });
        GlobalConstants.AppActions.Add(gameObject.name + "Pause", () => { });

        GlobalConstants.AppActions.Add(gameObject.name + "PlayButton", () => { });
    }

    private void AssignCamera(Camera obj)
    {
        this.GetComponent<Canvas>().worldCamera = obj;
    }

    public void OnEnable()
    {
        Debug.Log(gameObject.name);

        transform.GetChild(0).GetComponent<Button>().onClick.AddListener(()
        => VirtualwalkThrough.FireAppAction(gameObject.name + "Pause"));

        playButton.GetComponent<Button>().onClick.AddListener(()
                        => VirtualwalkThrough.FireAppAction(gameObject.name + "PlayButton"));

        closeButton.GetComponent<Button>().onClick.AddListener(()
                                         => VirtualwalkThrough.FireAppAction(gameObject.name + "CloseButton"));

        expandButton.GetComponent<Button>().onClick.AddListener(()
            => VirtualwalkThrough.FireAppAction(gameObject.name + "ExpandButton"));
    }

    private void Update()
    {
        if (currentVideoPlayer != null && currentVideoPlayer.isPlaying)
            videoSlider.value = (float)currentVideoPlayer.frame / (float)currentVideoPlayer.frameCount;

    }
    public void Setpurpose(object objectType)
    {

        currentObject = objectType;
        transform.GetChild(0).gameObject.SetActive(true);
        switch (objectType)
        {
            case VideoPlayer videoPlayer:
                currentVideoPlayer = videoPlayer;
                if (transform.parent.childCount > 1)
                {
                    playButton.SetActive(false);
                    closeButton.SetActive(true);
                }
                else
                {
                    playButton.SetActive(true);
                    closeButton.SetActive(false);
                    videoSlider.gameObject.SetActive(false);
                }


                GlobalConstants.AppActions[gameObject.name + "Pause"] =
                () =>
                {
                    Debug.Log("Pausing Here");
                    currentVideoPlayer.Pause();
                    if (transform.parent.childCount > 1)
                    {
                        closeButton.SetActive(true);
                    }
                    else
                    {
                        closeButton.SetActive(false);

                    }
                    playButton.SetActive(true);
                };



                GlobalConstants.AppActions[gameObject.name + "PlayButton"] =
                () =>
                {
                    PlayButton();

                };



                GlobalConstants.AppActions[gameObject.name + "CloseButton"] =
                () =>
                {
                    currentVideoPlayer.Stop();
                    if (transform.parent.childCount > 1)
                    {
                        transform.parent.GetChild(1).gameObject.SetActive(true);

                        transform.parent.GetChild(0).gameObject.SetActive(false);
                    }
                    else if (transform.parent.childCount == 1)
                    {
                        playButton.SetActive(true);
                        closeButton.SetActive(false);


                    }
                };

                VirtualwalkThrough.FireVideoSetup(currentObject,true);


                break;

            case Texture2D texture2D:
                closeButton.SetActive(true);
                playButton.SetActive(false);



                GlobalConstants.AppActions[gameObject.name + "CloseButton"] =
                () =>
                {
                    Debug.Log("Here");
                    if (transform.parent.childCount > 1)
                    {
                        transform.parent.GetChild(1).gameObject.SetActive(true);

                        transform.parent.GetChild(0).gameObject.SetActive(false);
                    }
                };


                break;
        }


            GlobalConstants.AppActions[gameObject.name + "ExpandButton"] =
            () =>
            {
                Debug.Log("Calling Here");

                FullScreen();

            };
        

    }



    private void PlayButton()
    {
        if (currentVideoPlayer.isPrepared)
        {
            Debug.Log("Playing preparing");




            loadingObject.SetActive(false);

            closeButton.SetActive(true);
            playButton.SetActive(false);
            currentVideoPlayer.Play();
            videoSlider.gameObject.SetActive(true);

        }
        else
        {

            Debug.Log("Playing Here");
            playButton.SetActive(false);
            // currentVideoPlayer.Play();
            currentVideoPlayer.Prepare();
            loadingObject.SetActive(true);
            currentVideoPlayer.prepareCompleted += CurrentVideoPlayer_prepareCompleted;
        }
    }

    private void CurrentVideoPlayer_prepareCompleted(VideoPlayer source)
    {
            Debug.Log("Preparation Done");

        currentVideoPlayer.prepareCompleted -= CurrentVideoPlayer_prepareCompleted;


        PlayButton();
    }

    private void OnDisable()
    {
        playButton.GetComponent<Button>().onClick.RemoveAllListeners();
        closeButton.GetComponent<Button>().onClick.RemoveAllListeners();
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
    public void FullScreen()
    {
        Debug.Log("Calling");
        VirtualwalkThrough.FireVideoSetup(currentObject,false);
    }
    public void ExitFullScreen()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        currentVideoPlayer.renderMode = VideoRenderMode.MaterialOverride;
        currentVideoPlayer.targetMaterialRenderer = this.GetComponent<MeshRenderer>();
    }

    private void OnDestroy()
    {
 
        VirtualwalkThrough.AsignvideoCamera -= AssignCamera;

    }
}
