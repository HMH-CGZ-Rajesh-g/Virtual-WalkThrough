using System.Collections;
using System.Collections.Generic;
using cdb.virtualwalkthrough;
using TMPro;
using UnityEngine;

public class ManagerCabinDataAssigner : MonoBehaviour
{
	public void AssignData(GameObject instatiated, ManagerCabin managerCabin)
	{



        instatiated.transform.GetComponentInChildren<TextMeshPro>().text = "Manager Room";

        AgendaDetails agenda_managerRoom = new AgendaDetails();
        agenda_managerRoom.AgendaSpeakers = managerCabin.AgendaSpeakers_ManagerCabin;
        agenda_managerRoom.AgendaTitle = managerCabin.AgendaTitle_ManagerCabin;
        agenda_managerRoom.ClientNames = "";
        agenda_managerRoom.Details = managerCabin.AgendaDetails_ManagerCabin;


        GlobalConstants.Agendas.Add("Floor_Manager_room", agenda_managerRoom);

        DataManager.instance.AddCameraActions("ManagerCabin",instatiated.transform.position + new Vector3(-1, 1, -4), new Vector3(0, -52, 0));
        //GlobalConstants.AppActions.Add("ManagerCabin", () => playerCamera.transform.parent.GetComponent<CharacterMovement>().SetPoision(instatiated.transform.position + new Vector3(-1, 1, -4), new Vector3(0, -52, 0)));

        //if(managerCabin.InputTitle_ManagerCabin!=null || managerCabin.InputTitle_ManagerCabin!=string.Empty)
        //instatiated.transform.GetComponentInChildren<TextMeshPro>().text = managerCabin.InputTitle_ManagerCabin;

        GameObject managerdoor = instatiated.transform.Find("Door_small").gameObject;
        managerdoor.name = "manager_door";
        managerdoor.AddComponent<Animator>().runtimeAnimatorController = DataManager.instance.singleDoor;
        GlobalConstants.AppActions.Add("manager_door", () => managerdoor.GetComponent<Animator>().SetTrigger("open"));

        DataManager.instance.LoadImageORVideo(managerCabin.WallWallpaper_ManagerCabin, instatiated.transform.Find(nameof(managerCabin.WallWallpaper_ManagerCabin)).gameObject);

        DataManager.instance.LoadImageORVideo(managerCabin.WallDisplay1_ManagerCabin, instatiated.transform.Find("TV_screen_1").gameObject);




     
	}
}
