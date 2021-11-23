using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FeedBackObserver : MonoBehaviour
{
   public  GameObject feedbaksDisplay;
    public TextMeshProUGUI feedbackText;
    private int currentfeedbackNumber;

    
    // Start is called before the first frame update




    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.D)&& !feedbaksDisplay.activeSelf)
        {
            Debug.Log("DisplayPanel");
            feedbaksDisplay.SetActive(true);
            currentfeedbackNumber=0;
            DisplayFeedback(0);
        }
    }

    public void loadCurrentfeedback(int value)
    {
        if(currentfeedbackNumber+value>=0 && currentfeedbackNumber + value<CustomerFeedbackManager.currentfeedbacks.Count)
        {
            currentfeedbackNumber += value;

            DisplayFeedback(currentfeedbackNumber);
        }
    }

    private void DisplayFeedback(int value)
    {
        Feedback feedback = CustomerFeedbackManager.currentfeedbacks[value];
        Debug.Log(feedback.clientName);
        feedbackText.text = "<size=17>"+feedback.feedback + "</size>\n\n"+feedback.clientName + "\n" + feedback.designation + "\n"+ feedback.company;
        
    }
    public void DeleteFeedback()
    {
        CustomerFeedbackManager.currentfeedbacks.RemoveAt(currentfeedbackNumber);

        if(CustomerFeedbackManager.currentfeedbacks.Count==0)
        {
            feedbackText.text = "";
        }
        else if (CustomerFeedbackManager.currentfeedbacks.Count> currentfeedbackNumber)
        {
            DisplayFeedback(currentfeedbackNumber);
        }
        else
        {
            DisplayFeedback(currentfeedbackNumber-1);

        }
    }
}
