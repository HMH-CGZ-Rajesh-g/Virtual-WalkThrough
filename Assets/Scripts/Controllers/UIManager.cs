using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject cursor;
    [SerializeField] GameObject helpIcon;
    [SerializeField] GameObject mapIcon;
    [SerializeField] GameObject Help;
    [SerializeField] GameObject Map;
    [SerializeField] CharacterMovement character;
    [SerializeField] CameraMovement camMove;
    [SerializeField] GameObject Fullscreen;
    public static bool isUIOpen = false;

    void OnEnable()
    {
        cursor.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void DisableWelcomeScreen()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        cursor.SetActive(true);
        character.enabled = true;
        camMove.enabled = true;
        helpIcon.SetActive(true);
        mapIcon.SetActive(true);
    }

    public void ShowHelp()
    {
        isUIOpen = true;
        Help.SetActive(true);
    }

    public void ShowMap()
    {
        isUIOpen = true;
        Map.SetActive(true);
    }

    public void Close()
    {
        isUIOpen = false;
        Help.SetActive(false);
        Map.SetActive(false);
    }

    public void CloseFullScreen()
    {
        isUIOpen = false;
        Fullscreen.SetActive(false);
    }

    public void CloseThanks()
    {
        Cursor.lockState = CursorLockMode.Locked;
        character.enabled = true;
        camMove.enabled = true;
        cursor.SetActive(true);
        helpIcon.SetActive(true);
        mapIcon.SetActive(true);
        transform.GetChild(8).gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Cursor.lockState = CursorLockMode.None;
            character.enabled = false;
            camMove.enabled = false;
            Close();
            cursor.SetActive(false);
            helpIcon.SetActive(false);
            mapIcon.SetActive(false);
            transform.GetChild(8).gameObject.SetActive(true);
        }
    }
}
