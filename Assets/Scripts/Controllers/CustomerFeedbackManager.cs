using System.Collections.Generic;
using TMPro;
using UnityEngine;
using cdb.virtualwalkthrough;
public class CustomerFeedbackManager : MonoBehaviour
{
    int count = 0;
    int dynamicCount = 0;

    public static List<Feedback> currentfeedbacks = new List<Feedback>();

    private void Awake()
    {
        VirtualwalkThrough.LoadFeedback += Initiate;
        VirtualwalkThrough.Feed_feedBack += VirtualwalkThrough_Feed_feedBack; 

    }

    private void VirtualwalkThrough_Feed_feedBack(Feedback obj)
    {

        HandleDynamicFeedback(obj);
        //new Feedback() { clientName = "MCD", meedtingId = 2, feedback = "nice one", company = "CTS", designation = "SAO" });


        
    }

    private void OnDestroy()
    {
        VirtualwalkThrough.LoadFeedback -= Initiate;
        VirtualwalkThrough.Feed_feedBack -= VirtualwalkThrough_Feed_feedBack;


    }

    public void Initiate(List<Feedback> feed)
    {
        if (feed != null)
        {
            currentfeedbacks= feed;
            if (feed.Count > 16)
            {
                feed.RemoveRange(16, feed.Count-16);
            }
            Debug.Log(feed.Count);
            for (int i = 0; i < feed.Count; i++)
            {
                UpdateFeedback(transform.GetChild(0).GetChild(i), feed[i]);
            }
        }
        else
        {
            Debug.Log("No feedback for wall");
        }
    }

    public void HandleDynamicFeedback(Feedback feedback)
    {

        Debug.Log("Checking for  dynamic feedback count");
        if (dynamicCount < 4)
        {
                UpdateFeedback(transform.GetChild(1).GetChild(dynamicCount), feedback);
                dynamicCount++;
        }
        else
        {
            Debug.Log("Feedback full");
        }
        currentfeedbacks.Add(feedback);
        VirtualwalkThrough.FireUpdateFeedBack(currentfeedbacks);
    }

    void UpdateFeedback(Transform obj, Feedback feedback)
    {
        obj.GetChild(1).GetComponent<TextMeshProUGUI>().text = feedback.feedback;
         string details = feedback.clientName;
        if (!string.IsNullOrEmpty(feedback.designation))
        {
            details += ", " + feedback.designation;
        }
        if (!string.IsNullOrEmpty(feedback.company))
        {
            details += "\n" + feedback.company;
        }
        obj.GetChild(2).GetComponent<TextMeshProUGUI>().text = details;
        obj.gameObject.SetActive(true);
    }



}
