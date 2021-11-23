using System.Collections;
using System.Collections.Generic;
using cdb.virtualwalkthrough;
using TMPro;
using UnityEngine;

public class StagingAreaDataAssigner : MonoBehaviour
{
	public void AssigningStagingArea(GameObject instatiated, StagingArea stagingArea)
	{
		instatiated.transform.GetComponentInChildren<TextMeshPro>().text = "StagingArea";
		string triggerName = instatiated.transform.Find("Floor_Staging_room").gameObject.name;
		AgendaDetails agenda = new AgendaDetails();
		agenda.AgendaSpeakers = stagingArea.AgendaSpeakers_StagingArea;
		agenda.AgendaTitle = stagingArea.AgendaTitle_StagingArea;
		agenda.ClientNames = stagingArea.ClientNames_StagingArea;
		agenda.Details = stagingArea.AgendaDetails_StagingArea;


		GlobalConstants.Agendas.Add("Floor_Staging_room", agenda);
		//VirtualwalkThrough.FireCollisisonTrigger();

		VirtualwalkThrough.FireUIManagerInput(stagingArea);
		DataManager.instance.AddCameraActions("StagingArea",new Vector3(-10, 1, 2.5f), new Vector3(0, 90, 0));
		//GlobalConstants.AppActions.Add("StagingArea", () => playerCamera.transform.parent.GetComponent<CharacterMovement>().SetPoision(new Vector3(-10, 1, 2.5f), new Vector3(0, 90, 0)));

		GameObject door = instatiated.transform.Find("DoorGlassDecor1").gameObject;
		door.name = "stagingArea_door";
		door.AddComponent<Animator>().runtimeAnimatorController = DataManager.instance.doubleDoor;
		GlobalConstants.AppActions.Add("stagingArea_door", () => door.GetComponent<Animator>().SetTrigger("open"));

		DataManager.instance.LoadVideos(stagingArea.WallDisplay_StagingArea, instatiated);

        DataManager.instance.LoadImageORVideo(stagingArea.WallLogo_StagingArea, instatiated.transform.Find(nameof(stagingArea.WallLogo_StagingArea)).gameObject);
        DataManager.instance.LoadImageORVideo(stagingArea.WallWallpaper_StagingArea, instatiated.transform.Find(nameof(stagingArea.WallWallpaper_StagingArea)).gameObject);

		GameObject TV2 = instatiated.transform.Find("TV_screen_2").gameObject;
		TV2.name = "Staging-tv2";
        DataManager.instance.LoadImageORVideo(stagingArea.Display2_StagingArea, TV2);
	}
}
