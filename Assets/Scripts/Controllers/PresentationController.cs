//Attach this script to your Canvas GameObject.
//Also attach a GraphicsRaycaster component to your canvas by clicking the Add Component button in the Inspector window.
//Also make sure you have an EventSystem in your hierarchy.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class PresentationController : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    public bool raycastEnabled;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Image[] iconSets;
    private GameObject experienceBooth;
    [SerializeField] GameObject[] solutions;
    private PresentationManager _pm;
    private bool isPaused;
    private bool isEZOpen = false;
    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
        _pm = transform.parent.GetComponent<PresentationManager>();
    }

    void Update()
    {
        if (raycastEnabled && !UIManager.isUIOpen)
        {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);
            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.name == "ExperienceBooth")
                {
                    result.gameObject.GetComponent<Image>().sprite = sprites[1];
                    experienceBooth = result.gameObject;
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        ShowExperienceBooth();
                    }
                }
                else if (result.gameObject.name == "BackButton")
                {
                    if (experienceBooth != null)
                    {
                        experienceBooth.GetComponent<Image>().sprite = sprites[0];
                    }
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        HideExperienceBooth();
                    }
                }
                else if (result.gameObject.name == "IconBg")
                {
                    if (experienceBooth != null)
                    {
                        experienceBooth.GetComponent<Image>().sprite = sprites[0];
                    }
                    IconHover(result.gameObject);
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        PerformIconAction(result.gameObject.transform.parent);
                    }
                }
                else if (result.gameObject.name == "Pause")
                {
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        if (isPaused)
                        {
                            result.gameObject.GetComponent<Image>().sprite = sprites[5];
                            _pm.PlayVideo();
                            isPaused = false;
                        }
                        else
                        {
                            result.gameObject.GetComponent<Image>().sprite = sprites[4];
                            _pm.PauseVideo();
                            isPaused = true;
                        }
                    }
                }
                else if (result.gameObject.name == "Close")
                {
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        _pm.MoveToNext();
                    }
                }
                else if (result.gameObject.name == "Cafe" || result.gameObject.name == "AutoGarage" || result.gameObject.name == "AgentsOffice" || result.gameObject.name == "CXOCabin" || result.gameObject.name == "Warehouse" || result.gameObject.name == "LivingRoom"  )
                {
                    ClearExperienceHovers();
                    result.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    if (Input.GetKey(KeyCode.Mouse0) && isEZOpen)
                    {
                        result.gameObject.GetComponent<Button>().onClick.Invoke();
                        HideExperienceBooth();
                        raycastEnabled = false;
                    }
                }
                else
                {
                    ClearHovers();
                    ClearExperienceHovers();
                }
            }
        }

    }

    private void PerformIconAction(Transform iconObject)
    {
        Camera.main.GetComponent<CameraMovement>().LinkSelection();
        iconObject.GetComponent<IconController>().RequestedAction();
    }

    public void ShowExperienceBooth()
    {
        foreach (Image iconset in iconSets)
        {
            iconset.transform.parent.gameObject.SetActive(false);
        }

        transform.GetChild(1).gameObject.SetActive(true);
        ClearHovers();
        Invoke("EnableLayers", 1f);
    }
    void EnableLayers()
    {
        isEZOpen = true;
    }

    public void HideExperienceBooth()
    {
        ClearExperienceHovers();
        transform.GetChild(1).gameObject.SetActive(false);
        ClearHovers();
        foreach (Image iconset in iconSets)
        {
            iconset.transform.parent.gameObject.SetActive(true);
        }
        isEZOpen = false;
    }

    public void IconHover(GameObject iconSet)
    {
        ClearHovers();
        iconSet.GetComponent<Image>().sprite = sprites[3];
        iconSet.transform.parent.GetChild(2).gameObject.SetActive(true);
    }

    void ClearHovers()
    {
        if (experienceBooth != null)
        {
            experienceBooth.GetComponent<Image>().sprite = sprites[0];
        }

        foreach (Image set in iconSets)
        {
            set.sprite = sprites[2];
            set.transform.parent.GetChild(2).gameObject.SetActive(false);
        }
    }

    private void ClearExperienceHovers()
    {
        foreach (GameObject solution in solutions)
        {
            solution.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}