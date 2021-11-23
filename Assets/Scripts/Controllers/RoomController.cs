using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class RoomController : MonoBehaviour
{
    [SerializeField] AudioController vNetController;
    [SerializeField] Transform[] solutionsets;
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    [SerializeField] RenderTexture TVTexture;
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject Teleporter;
    //[SerializeField] GameObject quad;
    //[SerializeField] GameObject close;
    //[SerializeField] GameObject pause;
    [SerializeField] Sprite[] sprites;
    public bool raycastEnabled;
    public VideoController vController;
    private bool isPaused = false;

    [DllImport("__Internal")]
    private static extern void OpenUrl(string url);

    private void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
    }

    internal void URLOpener(string url)
    {
        RemoveHover();
#if UNITY_EDITOR
        Application.OpenURL(url);
#else
        Debug.Log(url);
        OpenUrl(url);
#endif
    }

    public void PlayVideoOnScreen(string url)
    {

        RemoveHover();
#if UNITY_EDITOR
        url = "localhost:80/" + url;
        Application.OpenURL(url);
#else
        Debug.Log(url);
        OpenUrl(url);
#endif
        //quad.setactive(true);
        //vcontroller.playvideo(url, tvtexture);
        //pause.setactive(true);
        //close.setactive(true);
    }

    //private void OnVideoEnded(VideoPlayer source)
    //{
    //    quad.SetActive(false);
    //    pause.SetActive(false);
    //    close.SetActive(false);
    //}

    //public void PauseVideo()
    //{
    //    vController.GetComponent<VideoPlayer>().Pause();
    //}

    public void PlayVideo()
    {
        vController.GetComponent<VideoPlayer>().Play();
    }


    private void OnTriggerStay(Collider other)
    {
        // vController.GetComponent<VideoPlayer>().loopPointReached += OnVideoEnded;
        Teleporter.SetActive(true);
        raycastEnabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Teleporter.SetActive(false);
        vNetController.DropHighlight();
        //vController.GetComponent<VideoPlayer>().loopPointReached -= OnVideoEnded;
        raycastEnabled = false;
        RemoveHover();
    }

    void Update()
    {
        if (raycastEnabled && !UIManager.isUIOpen)
        {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(eventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);
            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.name == "Solution")
                {
                    HoverEffect(result.gameObject.transform);
                    if (Input.GetKey(KeyCode.Mouse0) && !result.gameObject.GetComponent<TVScreenController>().isDisabled)
                    {
                        PerformAction(result.gameObject.transform);
                    }
                }
                else
                {
                    RemoveHover();
                }
            }
        }
    }

    private void HoverEffect(Transform obj)
    {
        if (!obj.GetComponent<TVScreenController>().isDisabled)
        {
            obj.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void RemoveHover()
    {
        foreach (Transform solution in solutionsets)
        {
            if (!solution.GetComponent<TVScreenController>().isDisabled)
            {
                solution.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void PerformAction(Transform obj)
    {
        Camera.main.GetComponent<CameraMovement>().LinkSelection();
        obj.GetComponent<TVScreenController>().RequestedAction();
    }

    public void MoveToNext()
    {
        vController.GetComponent<VideoPlayer>().Stop();
        //quad.SetActive(false);
        //close.SetActive(false);
        //pause.SetActive(false);
    }
}
