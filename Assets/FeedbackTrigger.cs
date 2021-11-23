using System.Collections;
using System.Collections.Generic;
using cdb.virtualwalkthrough;
using UnityEngine;

public class FeedbackTrigger : MonoBehaviour
{
    public GameObject triggerText;

    private void OnTriggerEnter(Collider other)
    {
        if(!GlobalConstants.isAdmin)
        triggerText.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        triggerText.SetActive(false);
    }
}
