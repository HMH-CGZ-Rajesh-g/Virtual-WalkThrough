using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cdb.virtualwalkthrough;


public class MouseClickManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)&& DataManager.playerCamera!=null && GlobalConstants.isAdmin)

        {
            RaycastHit hit;
            Ray ray = DataManager.playerCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.transform.parent.GetComponent<Animator>())
                {
                    VirtualwalkThrough.FireAppAction(hit.collider.transform.parent.name);//.GetComponent<Animator>().SetTrigger("open");
                }
                else if (hit.collider != null && hit.collider.name.Contains("TeleportationRig"))
                {
                    Debug.Log(hit.collider.transform.position);
                    DataManager.playerCamera.transform.parent.transform.position= new Vector3(hit.collider.transform.position.x,1f,hit.collider.transform.position.z);
                    DataManager.playerCamera.transform.parent.rotation = hit.collider.gameObject.transform.rotation;
                    DataManager.playerCamera.transform.localRotation = Quaternion.Euler(Vector3.zero);
                }
                else if (hit.collider != null && hit.collider.name == "Standy")
                {
                    Debug.Log(hit.collider.transform.position);
                    DataManager.playerCamera.transform.parent.transform.position = new Vector3(hit.collider.transform.position.x, 1f, hit.collider.transform.position.z);
                    DataManager.playerCamera.transform.parent.rotation = hit.collider.gameObject.transform.rotation;
                    DataManager.playerCamera.transform.localRotation = Quaternion.Euler(Vector3.right*20);
                }
                else if (hit.collider != null && hit.collider.name == "FeedbackTrigger")
                {
                    Debug.Log("Feedback Triggered");
                }
            }
        }
    }
}
