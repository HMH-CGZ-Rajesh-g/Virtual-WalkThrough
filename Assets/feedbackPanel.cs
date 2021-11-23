using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class feedbackPanel : MonoBehaviour
{
   public InputField clientName,companyName,designation,feedback;
    public TextMeshProUGUI inputText; 

    // Start is called before the first frame update
    void OnEnable()
    {
        ClearInput();   
    }

    private void ClearInput()
    {
        clientName.text = "";
        companyName.text = "";
        designation.text = "";
        feedback.text = "";
    }

public void SendFeedBack()
    {
        if (clientName.text != string.Empty && companyName.text != string.Empty &&
            designation.text != string.Empty && feedback.text != string.Empty)
        {
            VirtualwalkThrough.FireFeed_FeedBack(
                new Feedback() { clientName = clientName.text, feedback = feedback.text, company = companyName.text, designation = designation.text });
            gameObject.SetActive(false);
        }
        else
        {
            inputText.gameObject.SetActive(false);
            Invoke("VirtualwalkThrough.EnableObject(inputText.gameObject,true)",2);
        }
    }
}
