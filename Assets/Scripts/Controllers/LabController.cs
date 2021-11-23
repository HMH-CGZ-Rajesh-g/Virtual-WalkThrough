using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LabController : MonoBehaviour
{
    [SerializeField] Image[] iconSets;
    [SerializeField] Button Back;
    [SerializeField] Button Expand;
    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject Teleporter;
    [SerializeField] Texture textureOriginal;
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    public EventSystem m_EventSystem;
    public bool raycastEnabled;
    public Material material;
    Sprite _sprite;
    public Image fullscreenImage;
    [DllImport("__Internal")]
    private static extern void OpenUrl(string url);
    // Start is called before the first frame update
    void Start()
    {
        material.mainTexture = textureOriginal;
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        textureOriginal = material.mainTexture;
    }

    private void OnTriggerStay(Collider other)
    {
        Teleporter.SetActive(true);
        raycastEnabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Teleporter.SetActive(false);
        ClearHovers();
        raycastEnabled = false;
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
    public void DisplayPicture(Sprite sprite)
    {
        _sprite = sprite;
        material.mainTexture = sprite.texture;
        Camera.main.GetComponent<CameraMovement>().mLock = true;
    }

    // Update is called once per frame
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
                if (result.gameObject.name == "IconBg")
                {
                    IconHover(result.gameObject);
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        PerformIconAction(result.gameObject.transform.parent);
                    }
                }
                else if (result.gameObject.name == "Back")
                {
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        BackAction();
                    }
                }
                else if (result.gameObject.name == "Fullscreen")
                {
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        ExpandAction();
                    }
                }
                else
                {
                    ClearHovers();
                }
            }
        }
    }

    public void IconHover(GameObject iconSet)
    {
        ClearHovers();
        iconSet.GetComponent<Image>().sprite = sprites[1];
        iconSet.transform.parent.GetChild(2).gameObject.SetActive(true);
    }

    void ClearHovers()
    {
        foreach (Image set in iconSets)
        {
            set.sprite = sprites[0];
            set.transform.parent.GetChild(2).gameObject.SetActive(false);
        }
    }
    private void PerformIconAction(Transform iconObject)
    {
        ClearHovers();
        iconObject.GetComponent<IconController>().RequestedAction();
    }

    public void ExpandAction()
    {
        Camera.main.GetComponent<CameraMovement>().LinkSelection();
        fullscreenImage.sprite = _sprite;
        fullscreenImage.SetNativeSize();
        fullscreenImage.gameObject.SetActive(true);
    }

    public void BackAction()
    {
        EnableIcons();
        material.mainTexture = textureOriginal;
    }

    public void DisableIcons()
    {
        foreach (Image icon in iconSets)
        {
            icon.transform.parent.gameObject.SetActive(false);
        }
        Back.gameObject.SetActive(true);
        Expand.gameObject.SetActive(true);
    }

    void EnableIcons()
    {
        Back.gameObject.SetActive(false);
        Expand.gameObject.SetActive(false);
        foreach (Image icon in iconSets)
        {
            icon.transform.parent.gameObject.SetActive(true);
        }
    }
}
