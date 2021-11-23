using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using cdb.virtualwalkthrough;
using System.Linq;

public class UIManager_VW : MonoBehaviour
{

    public TextMeshProUGUI companyName, welcomeText, agendaText;

    public Image clientImage;
    public GameObject actionsContainer;
    public GameObject StartPannel;
    public GameObject disableAccessPannel;

    public GameObject feedbackObject, thankYouObject, instructionsPanel;

    public List<Sprite> sideImages = new List<Sprite>();

    public GameObject agendaStagingArea,agendaHud;
    public Joystick movement, rotation;

    private void Start()
    {
        if (!GlobalConstants.isAdmin)
        {
            disableAccessPannel.SetActive(true);
        }
       
    }

    private void Awake()
    {
        VirtualwalkThrough.UIManagerInput += VirtualwalkThrough_UIManagerInput;
        VirtualwalkThrough.AreaActions += VirtualwalkThrough_AreaActions;
        VirtualwalkThrough.GameKeys += VirtualwalkThrough_GameKeys;
        VirtualwalkThrough.CurrentAgendaDetails += ShowAgenda;
        VirtualwalkThrough.Collisison_Trigger += VirtualwalkThrough_Collisison_Trigger;
        GlobalConstants.rotation = rotation;
        GlobalConstants.movement = movement;
#if UNITY_WEBGL
                rotation.gameObject.SetActive(false);
        movement.gameObject.SetActive(false);       
#endif


    }

    private void VirtualwalkThrough_Collisison_Trigger(string obj)
    {
        if(GlobalConstants.Agendas.ContainsKey(obj))
        {
            VirtualwalkThrough.FireCurrentAgendaDetails(GlobalConstants.Agendas[obj]);
        }
    }

    private void VirtualwalkThrough_GameKeys(KeyCode keyCode)
    {

        switch (keyCode)
        {
            case KeyCode.C:
                if (!feedbackObject.activeSelf)
                    DisableAllOjects();

                feedbackObject.SetActive(true);
                return;
            case KeyCode.Escape:
                if (!instructionsPanel.activeSelf)
                    DisableAllOjects();
                instructionsPanel.SetActive(true);
                return;
            case KeyCode.V:
                // Key_V.Invoke();

                return;
            case KeyCode.T:
                if (!feedbackObject.activeSelf && !instructionsPanel.activeSelf)
                {
                    DisableAllOjects();

                    thankYouObject.SetActive(true);
                }
                return;

        }
    }

    void DisableAllOjects()
    {
        feedbackObject.SetActive(false);
        thankYouObject.SetActive(false);
        instructionsPanel.SetActive(false);


    }

    public void Exit()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
    Application.OpenURL("about:blank");
#else
        Application.Quit();
#endif
    }
    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.C)&&!feedbackObject.activeSelf)
        //{
        //    feedbackObject.SetActive(true);
        //}
        //if (Input.GetKeyDown(KeyCode.V) && !thankYouObject.activeSelf)
        //{
        //    thankYouObject.SetActive(true);
        //}
    }

    private void VirtualwalkThrough_AreaActions(Dictionary<string, Action> obj)
    {
        for (int i = 0; i < obj.Count; i++)
        {
            if (actionsContainer.transform.childCount - 1 < i)
            {
                Instantiate(actionsContainer.transform.GetChild(i - 1).gameObject, actionsContainer.transform);
            }
            string title = obj.ElementAt(i).Key;
            actionsContainer.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = obj.ElementAt(i).Key;
            actionsContainer.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite =sideImages[UnityEngine.Random.Range(0, sideImages.Count-1)] ;

            actionsContainer.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => VirtualwalkThrough.FireAppAction(title));//obj.ElementAt(i).Value.Invoke

        }
    }
        public void ControlContainer()
    {
        if (actionsContainer.activeInHierarchy)
        {
            actionsContainer.SetActive(false);
        }
        else
        {
            actionsContainer.SetActive(true);
        }
    }


    private void VirtualwalkThrough_UIManagerInput(StagingArea stagingArea)
    {
        clientImage.gameObject.SetActive(false);

        StartPannel.transform.GetChild(0).gameObject.SetActive(true);


        companyName.text = stagingArea.ClientOrganisation_StagingArea;


        agendaStagingArea.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Client";
        agendaStagingArea.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Organization";
        agendaStagingArea.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Agenda";
        agendaStagingArea.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Details";
        agendaStagingArea.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Presented by";


        agendaStagingArea.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = stagingArea.ClientNames_StagingArea;
        agendaStagingArea.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = stagingArea.ClientOrganisation_StagingArea;
        agendaStagingArea.transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = stagingArea.AgendaTitle_StagingArea;
        agendaStagingArea.transform.GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>().text = stagingArea.AgendaDetails_StagingArea;
        agendaStagingArea.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = stagingArea.AgendaSpeakers_StagingArea;
        welcomeText.text = 
            "\n<size=25> Client              : " + stagingArea.ClientNames_StagingArea +
            "\n Organization   : " + stagingArea.ClientOrganisation_StagingArea
           // +"\n  " + stagingArea.AgendaSpeakers_StagingArea + "</size></b>"
            + "\n Agenda           : " + stagingArea.AgendaTitle_StagingArea //+ "</size>"
            + "\n Details            : " + stagingArea.AgendaDetails_StagingArea// + "</size>"
            + "\n Presented by  : " + stagingArea.AgendaSpeakers_StagingArea + "</size>";





    }

    void ShowAgenda(AgendaDetails stagingArea)
    {



        agendaHud.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Presented by";
        agendaHud.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Agenda";
        agendaHud.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Details";
        //agendaHud.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Details";


        agendaHud.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = stagingArea.AgendaSpeakers;
        agendaHud.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = stagingArea.AgendaTitle;
        agendaHud.transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = stagingArea.Details;
        //agendaHud.transform.GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>().text = stagingArea.Details;

        agendaText.text =
    "\n<size=40>Presented by :  " + stagingArea.AgendaSpeakers + "</size>"

           + "\n\n<size=30> Client              : " + stagingArea.ClientNames
            + "\n Agenda           : " + stagingArea.AgendaTitle //+ "</size>"
            + "\n Details            : " + stagingArea.Details + "</size>";
    }


    private void OnDestroy()
    {
        VirtualwalkThrough.UIManagerInput -= VirtualwalkThrough_UIManagerInput;
        VirtualwalkThrough.AreaActions -= VirtualwalkThrough_AreaActions;
        VirtualwalkThrough.GameKeys -= VirtualwalkThrough_GameKeys;
        VirtualwalkThrough.CurrentAgendaDetails -= ShowAgenda;
        VirtualwalkThrough.Collisison_Trigger -= VirtualwalkThrough_Collisison_Trigger;




    }

}
